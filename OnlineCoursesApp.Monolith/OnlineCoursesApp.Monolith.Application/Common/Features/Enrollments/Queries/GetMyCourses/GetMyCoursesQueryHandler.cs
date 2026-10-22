using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Queries.GetMyCourses;
public class GetMyCoursesQueryHandler(
    IEnrollmentRepository enrollmentRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetMyCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> Handle(GetMyCoursesQuery request, CancellationToken cancellationToken)
    {
        if (currentUserService.Role != RoleName.Student)
            throw new ForbiddenException("The list of your courses is available only to students.");

        var cources = await enrollmentRepository.GetUserCoursesAsync(currentUserService.UserId, cancellationToken);

        return cources.Select(c => new CourseDto(c.Id, c.Title, c.Description, c.AuthorId,
            c.Category, c.Price, c.CreatedAt)).ToList();
    }
}