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
    public async Task<ActionResult<string>> Login(LoginRequest login)
    {

        var token = _authService.Login(login.Username, login.Password);

        if (token == null)
        {
            return Unauthorized();
        }
        else
        {
            return Ok(token);
        }
    }
}