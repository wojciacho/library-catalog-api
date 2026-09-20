using LibraryCatalog.Services;
using Microsoft.Extensions.Configuration;

public class AuthServiceTests
{
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
        _service = new AuthService(configuration);
    }

    [Fact]
    public void Login_WithValidCredentials_ReturnsToken()
    {
        // Act
        var result = _service.Login("admin", "password");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Login_WithInvalidUsername_ReturnsNull()
    {
        // Act
        var result = _service.Login("dummy", "dummy");

        // Assert 
        Assert.Null(result);
    }

    [Fact]
    public void Login_WithInvalidPassword_ReturnsNull()
    {
        // Act
        var result = _service.Login("admin", "dummy");

        // Assert
        Assert.Null(result);
    }
}