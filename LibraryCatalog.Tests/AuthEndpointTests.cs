using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using LibraryCatalog.Models;

namespace LibraryCatalog.Tests;

public class AuthEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/auth/login", new LoginRequest("admin", "password"));
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(token);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/auth/login", new LoginRequest("dummy", "dummy"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/auth/login", new LoginRequest("admin", "dummy"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}