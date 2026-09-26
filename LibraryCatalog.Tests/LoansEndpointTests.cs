using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using LibraryCatalog.Models;

namespace LibraryCatalog.Tests;

public class LoansEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoansEndpointTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<(int bookId, int memberId)> SetupAsync()
    {
        await _client.PostAsJsonAsync("/auth/register", new RegisterRequest("admin", "password"));
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", new LoginRequest("admin", "password"));
        var token = await loginResponse.Content.ReadAsStringAsync();

        Assert.NotNull(token);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var bookResponse = await _client.PostAsJsonAsync("/Books", new Book { Title = "Test book", Author = "Test Author", IsAvailable = true });
        Assert.Equal(HttpStatusCode.OK, bookResponse.StatusCode);
        var bookId = (await bookResponse.Content.ReadFromJsonAsync<Book>())!.Id;

        var memberResponse = await _client.PostAsJsonAsync("/Members", new Member { Name = "Wojciech", Email = "wojciech@test.com" });
        Assert.Equal(HttpStatusCode.OK, memberResponse.StatusCode);
        var memberId = (await memberResponse.Content.ReadFromJsonAsync<Member>())!.Id;

        return (bookId, memberId);
    }

    private async Task<int> BorrowAsync()
    {
        var (bookId, memberId) = await SetupAsync();

        var loanResponse = await _client.PostAsJsonAsync("/Loans", new BorrowRequest(bookId, memberId));

        var loanId = (await loanResponse.Content.ReadFromJsonAsync<Loan>())!.Id;

        return loanId;
    }

    [Fact]
    public async Task GetLoans_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/Loans");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostLoans_WithToken_ReturnsOk()
    {
        // Arrange
        var (bookId, memberId) = await SetupAsync();

        // Act
        var loanResponse = await _client.PostAsJsonAsync("/Loans", new BorrowRequest(bookId, memberId));

        // Assert
        Assert.Equal(HttpStatusCode.OK, loanResponse.StatusCode);
    }

    [Fact]
    public async Task GetLoanById_WithToken_ReturnsOk()
    {
        // Arrange
        var loanId = await BorrowAsync();

        // Act
        var loanResponse = await _client.GetAsync($"/Loans/{loanId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, loanResponse.StatusCode);
    }

    [Fact]
    public async Task ReturnLoan_WithToken_ReturnsOk()
    {
        // Arrange
        var loanId = await BorrowAsync();

        // Act
        var loanResponse = await _client.PutAsync($"/Loans/{loanId}/return", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, loanResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteLoan_WithToken_ReturnsNoContent()
    {
        // Arrange
        var loanId = await BorrowAsync();

        // Act
        var loanResponse = await _client.DeleteAsync($"/Loans/{loanId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, loanResponse.StatusCode);
    }
}