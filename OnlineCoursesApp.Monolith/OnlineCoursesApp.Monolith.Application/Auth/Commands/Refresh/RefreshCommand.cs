using MediatR;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthResponseDto>;