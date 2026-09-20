using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using LibraryCatalog.Models;
namespace LibraryCatalog.Tests;

public class MembersEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MembersEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AuthenticateAsync()
    {
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", new LoginRequest("admin", "password"));
        var token = await loginResponse.Content.ReadAsStringAsync();

        Assert.NotNull(token);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task GetMembers_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/Members");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMembers_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await _client.GetAsync("/Members");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMemberById_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();
        var memberResponse = await _client.PostAsJsonAsync("/Members", new Member { Name = "Wojciech", Email = "wojciech@test.com" });
        Assert.Equal(HttpStatusCode.OK, memberResponse.StatusCode);
        var memberId = (await memberResponse.Content.ReadFromJsonAsync<Member>())!.Id;

        // Act
        var response = await _client.GetAsync($"/Members/{memberId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostMember_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await _client.PostAsJsonAsync("/Members", new Member { Name = "Wojciech", Email = "wojciech@test.com" });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutMember_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();
        var memberResponse = await _client.PostAsJsonAsync("/Members", new Member { Name = "Wojciech", Email = "wojciech@test.com" });
        Assert.Equal(HttpStatusCode.OK, memberResponse.StatusCode);
        var memberId = (await memberResponse.Content.ReadFromJsonAsync<Member>())!.Id;

        // Act
        var updateMember = new Member { Name = "Wojciech updated", Email = "wojciech@updated.com" };
        var response = await _client.PutAsJsonAsync($"/Members/{memberId}", updateMember);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMember_WithToken_ReturnsNoContent()
    {
        // Arrange
        await AuthenticateAsync();
        var memberResponse = await _client.PostAsJsonAsync("/Members", new Member { Name = "Wojciech", Email = "wojciech@test.com" });
        Assert.Equal(HttpStatusCode.OK, memberResponse.StatusCode);
        var memberId = (await memberResponse.Content.ReadFromJsonAsync<Member>())!.Id;

        // Act
        var response = await _client.DeleteAsync($"/Members/{memberId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}