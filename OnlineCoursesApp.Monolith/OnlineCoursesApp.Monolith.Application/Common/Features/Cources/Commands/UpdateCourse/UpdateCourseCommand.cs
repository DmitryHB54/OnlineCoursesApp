using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;

public record UpdateCourseCommand(
    Guid Id,
    string Title,
    string Description,
    CourseCategory Category,
    decimal Price) : IRequest<CourseDto>;