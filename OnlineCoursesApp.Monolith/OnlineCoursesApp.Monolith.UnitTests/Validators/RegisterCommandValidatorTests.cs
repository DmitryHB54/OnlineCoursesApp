using FluentAssertions;
using OnlineCoursesApp.Monolith.Application.Auth.Commands.Register;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.UnitTests.Validators;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidStudentData_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "Password123!", "Ivan", RoleName.Student);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_AdminRegistration_ShouldHaveError()
    {
        // Arrange
        var command = new RegisterCommand("admin@test.com", "Password123!", "Admin", RoleName.Admin);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Contains("Registration with admin role is prohibited."));
    }

    [Fact]
    public void Validate_ShortPassword_ShouldHaveError()
    {
        // Arrange
        var command = new RegisterCommand("test@test.com", "123", "Ivan", RoleName.Student);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}