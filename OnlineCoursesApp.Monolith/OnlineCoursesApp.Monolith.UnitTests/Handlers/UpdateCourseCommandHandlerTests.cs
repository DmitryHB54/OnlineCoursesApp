using Moq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;
using OnlineCoursesApp.Monolith.Application.MappingProfile;

namespace OnlineCoursesApp.Monolith.UnitTests.Handlers;

public class UpdateCourseCommandHandlerTests
{
    private readonly Mock<ICourseRepository> _courseRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ICurrentUserService> _currentUserMock = new();
    private readonly IMapper _mapper;
    private readonly UpdateCourseCommandHandler _handler;

    public UpdateCourseCommandHandlerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CourseProfile>();
        }, NullLoggerFactory.Instance);

        _mapper = mapperConfiguration.CreateMapper();

        _handler = new UpdateCourseCommandHandler(_courseRepoMock.Object, _unitOfWork.Object, _currentUserMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_TeacherEditingForeignCourse_ShouldThrowForbiddenException()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        var foreignAuthorId = Guid.NewGuid(); 

        var existingCourse = new Course { Id = courseId, AuthorId = foreignAuthorId };

        // Настраиваем моки
        _courseRepoMock.Setup(r => r.GetByIdForUpdateAsync(courseId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingCourse);

        _currentUserMock.Setup(u => u.Role).Returns(RoleName.Teacher);
        _currentUserMock.Setup(u => u.UserId).Returns(teacherId);

        var command = new UpdateCourseCommand(courseId, "New Title", "Desc", CourseCategory.Development, 100);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("You can't edit someone else's course.");
    }

    [Fact]
    public async Task Handle_AdminEditingForeignCourse_ShouldUpdateSuccessfully()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var existingCourse = new Course { Id = courseId, AuthorId = Guid.NewGuid() };

        _courseRepoMock.Setup(r => r.GetByIdForUpdateAsync(courseId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingCourse);

        _currentUserMock.Setup(u => u.Role).Returns(RoleName.Admin); 

        var command = new UpdateCourseCommand(courseId, "New Title", "Desc", CourseCategory.Development, 100);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Title.Should().Be("New Title");
        _unitOfWork.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}