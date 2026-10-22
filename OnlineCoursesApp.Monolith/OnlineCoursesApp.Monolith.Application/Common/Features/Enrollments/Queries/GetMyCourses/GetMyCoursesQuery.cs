using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Queries.GetMyCourses;
public record GetMyCoursesQuery : IRequest<List<CourseDto>>;