namespace OnlineCoursesApp.Monolith.Domain.Entities;

public class FileInfo
{
    public string RelativePath { get; set; } = default!;
    public string OriginalName { get; set; } = default!;
    public long SizeBytes { get; set; }
    public string Extension { get; set; } = default!;

    private FileInfo() { }

    public FileInfo(string relativePath, string originalName, long sizeBytes, string extension)
    {
        RelativePath = relativePath;
        OriginalName = originalName;
        SizeBytes = sizeBytes;
        Extension = extension;
    }
}