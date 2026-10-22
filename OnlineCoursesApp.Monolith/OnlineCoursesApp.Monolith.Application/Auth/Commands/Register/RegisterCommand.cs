using MediatR;
using OnlineCoursesApp.Monolith.Application.Auth.Dtos;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    RoleName Role) : IRequest<AuthResponseDto>;