using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseById;
public record GetCourseByIdQuery(Guid Id) : IRequest<CourseDto>;