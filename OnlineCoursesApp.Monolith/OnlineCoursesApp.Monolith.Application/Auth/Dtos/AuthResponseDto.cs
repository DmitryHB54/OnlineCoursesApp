namespace OnlineCoursesApp.Monolith.Application.Auth.Dtos;

public record AuthResponseDto(string AccessToken, DateTime ExpiresAt);