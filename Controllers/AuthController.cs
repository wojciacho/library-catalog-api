using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Services;
using LibraryCatalog.Models;

namespace LibraryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync(LoginRequest login)
    {

        var token = await _authService.LoginAsync(login.Username, login.Password);

        if (token == null)
        {
            return Unauthorized();
        }
        else
        {
            return Ok(token);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<bool>> RegisterAsync(RegisterRequest register)
    {
        var isRegistered = await _authService.RegisterAsync(register.Username, register.Password);

        if (!isRegistered)
        {
            return Conflict();
        }
        else
        {
            return Ok(isRegistered);
        }
    }
}