namespace OnlineCoursesApp.Monolith.Application.Auth.Dtos;

public record AuthResponseDto(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);