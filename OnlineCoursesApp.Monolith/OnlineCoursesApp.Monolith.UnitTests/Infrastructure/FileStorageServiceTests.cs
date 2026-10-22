using FluentAssertions;
using Microsoft.Extensions.Options;
using OnlineCoursesApp.Monolith.Infrastructure.Storage;

namespace OnlineCoursesApp.Monolith.UnitTests.Infrastructure
{
    public class FileStorageServiceTests
    {
        private readonly FileStorageService _service;

        public FileStorageServiceTests()
        {
            var options = Options.Create(new StorageOptions { RootPath = Path.GetTempPath() + "test_assets" });
            _service = new FileStorageService(options);
        }

        [Fact]
        public async Task SaveAsync_ExeFile_ShouldThrowException()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            Func<Task> act = async () => await _service.SaveAsync(Guid.NewGuid(), stream, "virus.exe", CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Extension '.exe' isn't supported.");
        }

        [Fact]
        public async Task OpenReadAsync_PathTraversalAttempt_ShouldThrowException()
        {
            // Arrange
            var maliciousPath = "../../../etc/passwd"; 

            // Act
            Func<Task> act = async () => await _service.OpeanReadAsync(maliciousPath, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Attempt to exit the storage limits.");
        }
    }
}