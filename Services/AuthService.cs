using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryCatalog.Models;
using LibraryCatalog.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LibraryCatalog.Services;

public class AuthService : IAuthService
{

    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user != null)
        {
            return false;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var newUser = new User { Username = username, PasswordHash = hashedPassword };
        await _userRepository.AddAsync(newUser);
        return true;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            return null;
        }

        var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!isPasswordCorrect)
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiryMinutes")),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}