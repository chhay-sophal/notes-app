using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, AuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        var result = await _authService.Register(model);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        var token = await _authService.Login(model);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await  _authService.Logout();
        return Ok(new { message = "User logged out successfully" });
    }

    private string GenerateJwtToken(User user)
    {
        var tokenString = _authService.GenerateJwtToken(user);

        return tokenString;
    }
}