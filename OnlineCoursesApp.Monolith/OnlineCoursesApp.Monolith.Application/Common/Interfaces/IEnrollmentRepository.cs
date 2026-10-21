namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;
public interface IEnrollmentRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid courseId, CancellationToken ct);
}