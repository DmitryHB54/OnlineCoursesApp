using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;

public class UpdateCourseCommandHandler 
    : IRequestHandler<UpdateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdForUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if (_currentUserService is { Role: RoleName.Teacher } && course.AuthorId != _currentUserService.UserId)
        { 
            throw new ForbiddenException("You can't edit someone else's course.");
        }

        course.Title = request.Title.Trim();
        course.Description = request.Description.Trim();
        course.Category = request.Category;
        course.Price = request.Price;

        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CourseDto>(course);
    }
}