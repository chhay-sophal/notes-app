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
        try
        {
            var result = await _authService.Register(model);

            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"{ex.Message}" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        try
        {
            var token = await _authService.Login(model);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await  _authService.Logout();
        return Ok(new { message = "User logged out successfully" });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser()
    {
        try {
            var userInfo = await _authService.GetLoggedInUser(User);
            return Ok(userInfo);
        }
        catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}