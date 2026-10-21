namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;
public record StoredFileResult(string RelativePath, long SizeBytes, string Extension);
public interface IFileStorageService
{
    Task<StoredFileResult> SaveAsync(
        Guid courseId, Stream content, string originalFileName, CancellationToken cancellationToken);

    Task<Stream> OpeanReadAsync(string relativePath, CancellationToken cancellationToken);
    Task DeleteAsync(string relativePath, CancellationToken cancellationToken);
    Task DeleteCourseFolderAsync(Guid courseId, CancellationToken cancellationToken);
    Task<Stream> CreateCourseArchiveAsync(Guid courseId, IEnumerable<string> relativePaths, CancellationToken cancellationToken);
}