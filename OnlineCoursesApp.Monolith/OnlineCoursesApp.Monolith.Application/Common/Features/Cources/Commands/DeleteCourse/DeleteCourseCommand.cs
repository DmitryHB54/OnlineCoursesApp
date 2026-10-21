using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : IRequest<Unit>;