using LibraryCatalog.Models;
using LibraryCatalog.Repositories;
using LibraryCatalog.Services;
using Microsoft.Extensions.Configuration;
using Moq;

public class AuthServiceTests
{

    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var configValues = new Dictionary<string, string?>
        {
        { "Jwt:Key", "aaaa-bbbb-cccc-dddd-eeee-ffff-gggg-hhhh" },
        { "Jwt:Issuer", "TestIssuer" },
        { "Jwt:Audience", "TestAudience" },
        { "Jwt:ExpiryMinutes", "60" }
        };

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
        _mockUserRepository = new Mock<IUserRepository>();
        _service = new AuthService(configuration, _mockUserRepository.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var user = new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
        _mockUserRepository.Setup(u => u.GetByUsernameAsync("admin")).ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync("admin", "password");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidUsername_ReturnsNull()
    {
        // Arrange
        _mockUserRepository.Setup(u => u.GetByUsernameAsync("dummy")).ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginAsync("dummy", "dummy");

        // Assert 
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
        _mockUserRepository.Setup(u => u.GetByUsernameAsync("admin")).ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync("admin", "dummy");

        // Assert
        Assert.Null(result);
    }
}