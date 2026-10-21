using Microsoft.Extensions.Options;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using System.IO.Compression;

namespace OnlineCoursesApp.Monolith.Infrastructure.Storage;
public class FileStorageService : IFileStorageService
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
         ".pdf", ".jpg", ".jpeg", ".webp", ".png", ".doc", ".docx"
    };

    private readonly string _rootPath;
    public FileStorageService(IOptions<StorageOptions> options)
    {
        _rootPath = Path.GetFullPath(options.Value.RootPath);
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<StoredFileResult> SaveAsync(
        Guid courseId, Stream content, string originalFileName, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"Extension '{extension}' isn't supported.");

        var courseFolder = Path.Combine(_rootPath, courseId.ToString());
        Directory.CreateDirectory(courseFolder);

        var diskName = $"{Guid.NewGuid()}{extension}";
        var absolutePath = Path.Combine(courseFolder, diskName);
        var relativePath = Path.Combine(courseId.ToString(), diskName).Replace('\\', '/');

        await using(var target = File.Create(absolutePath))
        {
            await content.CopyToAsync(target, cancellationToken);
        }

        var size = new FileInfo(absolutePath).Length;
        return new StoredFileResult(relativePath, size, extension);
    }
    public Task<Stream> OpeanReadAsync(string relativePath, CancellationToken cancellationToken)
    {
        string absolutePath = ResolveSafePath(relativePath);
        if (!File.Exists(absolutePath))
            throw new FileNotFoundException("File not found on disk.", absolutePath);

        Stream stream = File.OpenRead(absolutePath);
        return Task.FromResult(stream);
    }
    public Task DeleteAsync(string relativePath, CancellationToken cancellationToken)
    {
        string absolutePath = ResolveSafePath(relativePath);
        if (File.Exists(absolutePath))
            File.Delete(absolutePath);
        return Task.CompletedTask;
    }
    public Task DeleteCourseFolderAsync(Guid courseId, CancellationToken cancellationToken)
    {
        string folder = Path.Combine(_rootPath, courseId.ToString());
        if (Directory.Exists(folder))
            Directory.Delete(folder, recursive: true);
        return Task.CompletedTask;
    }

    public Task<Stream> CreateCourseArchiveAsync(Guid courseId, IEnumerable<string> relativePaths, CancellationToken cancellationToken)
    {
        var ms = new MemoryStream();
        using(var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach(var relPath in relativePaths)
            {
                var absolutePath = ResolveSafePath(relPath);
                if (!File.Exists(absolutePath)) continue;

                var entryName = Path.GetFileName(relPath);
                var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                using var source = File.OpenRead(absolutePath);
                source.CopyTo(entryStream);
            }
        }
        ms.Position = 0;
        return Task.FromResult<Stream>(ms);
    }

    private string ResolveSafePath(string relativePath)
    {
        var absolute = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        if (!absolute.StartsWith(_rootPath, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Attempt to exit the storage limits.");
        return absolute;
    }
}