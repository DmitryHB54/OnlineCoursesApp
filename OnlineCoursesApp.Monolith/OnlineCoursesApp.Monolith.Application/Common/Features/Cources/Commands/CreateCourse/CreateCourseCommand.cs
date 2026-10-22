using MediatR;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.CreateCourse;

public record CreateCourseCommand(
    string Title,
    string Description,
    CourseCategory Category,
    decimal Price) : IRequest<CourseDto>;