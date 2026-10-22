namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;

public record EnrollmentDto(
    Guid CourseId,
    string CourseTitle,
    DateTimeOffset EnrolledAt);