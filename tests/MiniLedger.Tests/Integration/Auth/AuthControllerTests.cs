using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MiniLedger.Api.DTOs.Auth;
using MiniLedger.Tests.Helpers;

namespace MiniLedger.Tests.Integration.Auth;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Return_Ok()
    {
        // Arrange
        var dto = new RegisterDto
        {
            FullName = "Test User",
            UserName = "testuser",
            Email = "testuser@example.com",
            Password = "Test123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_Should_Return_Token()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            FullName = "Login User",
            UserName = "loginuser",
            Email = "loginuser@example.com",
            Password = "Test123"
        };

        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new LoginDto
        {
            UserName = "loginuser",
            Password = "Test123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrWhiteSpace();
    }
}