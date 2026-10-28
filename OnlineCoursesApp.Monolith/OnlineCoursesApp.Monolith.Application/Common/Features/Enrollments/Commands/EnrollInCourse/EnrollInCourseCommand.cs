using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;

public record EnrollInCourseCommand(Guid CourseId) : IRequest<EnrollmentDto>;