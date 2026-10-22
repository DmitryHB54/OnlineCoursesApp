using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;
public class UpdateCourseCommandHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateCourseCommand, CourseDto>
{
    public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdForUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if (currentUserService.Role == RoleName.Teacher && course.AuthorId != currentUserService.UserId)
            throw new ForbiddenException("You can't edit someone else's course.");

        course.Title = request.Title.Trim();
        course.Description = request.Description.Trim();
        course.Category = request.Category;
        course.Price = request.Price;

        courseRepository.Update(course);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CourseDto(course.Id, course.Title, course.Description,
            course.AuthorId, course.Category, course.Price, course.CreatedAt);
    }
}