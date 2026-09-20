using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using LibraryCatalog.Models;

namespace LibraryCatalog.Tests;

public class BooksEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BooksEndpointTests(CustomWebApplicationFactory factory)
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
    public async Task GetBooks_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/Books");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetBooks_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await _client.GetAsync("/Books");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBookById_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();
        var bookResponse = await _client.PostAsJsonAsync("/Books", new Book { Title = "Test book", Author = "Test Author", IsAvailable = true });
        Assert.Equal(HttpStatusCode.OK, bookResponse.StatusCode);
        var bookId = (await bookResponse.Content.ReadFromJsonAsync<Book>())!.Id;

        // Act
        var response = await _client.GetAsync($"/Books/{bookId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostBook_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await _client.PostAsJsonAsync("/Books", new Book { Title = "Test book", Author = "Test Author", IsAvailable = true });

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutBook_WithToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();
        var bookResponse = await _client.PostAsJsonAsync("/Books", new Book { Title = "Test book", Author = "Test Author", IsAvailable = true });
        Assert.Equal(HttpStatusCode.OK, bookResponse.StatusCode);
        var bookId = (await bookResponse.Content.ReadFromJsonAsync<Book>())!.Id;

        // Act
        var updateBook = new Book { Title = "Test book updated", Author = "Test Author updated" };
        var response = await _client.PutAsJsonAsync($"/Books/{bookId}", updateBook);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_WithToken_ReturnsNoContent()
    {
        // Arrange
        await AuthenticateAsync();
        var bookResponse = await _client.PostAsJsonAsync("/Books", new Book { Title = "Test book", Author = "Test Author", IsAvailable = true });
        Assert.Equal(HttpStatusCode.OK, bookResponse.StatusCode);
        var bookId = (await bookResponse.Content.ReadFromJsonAsync<Book>())!.Id;

        // Act
        var response = await _client.DeleteAsync($"/Books/{bookId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}