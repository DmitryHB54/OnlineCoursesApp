using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;
public record CancelEnrollmentCommand(Guid CourseId) : IRequest<Unit>;