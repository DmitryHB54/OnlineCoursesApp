using MediatR;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Login;
public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;