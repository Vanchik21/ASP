using Carsharing.Data.Models;
using Carsharing.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthApiController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthApiController(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public record RegisterDto(string Email, string Password, string? FirstName, string? LastName);
    public record LoginDto(string Email, string Password);
    public record LoginResponseDto(string Token, string Email, string FirstName, string LastName, IList<string> Roles, DateTime ExpiresAt);
    public record ChangePasswordDto(string CurrentPassword, string NewPassword);
    public record UserProfileDto(string Id, string Email, string FirstName, string LastName, DateTime RegisteredAt, IList<string> Roles);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { message = "Email and Password are required" });
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "User with this email already exists" });
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        await _userManager.AddToRoleAsync(user, "User");
        
        return Created($"api/auth/profile", new 
        { 
            user.Id, 
            user.Email, 
            user.FirstName, 
            user.LastName,
            message = "User registered successfully. Please login to get your token."
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { message = "Email and Password are required" });
        }

        Console.WriteLine($"Login attempt for: {dto.Email}");

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            Console.WriteLine($"User not found: {dto.Email}");
            return Unauthorized(new { message = "Invalid credentials" });
        }

        Console.WriteLine($"User found: {user.Email}, checking password...");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
        
        if (!result.Succeeded)
        {
            Console.WriteLine($"Password check failed. IsLockedOut: {result.IsLockedOut}");
            if (result.IsLockedOut)
            {
                return Unauthorized(new { message = "Account is locked out. Please try again later." });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }

        Console.WriteLine($"Login successful for: {dto.Email}");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);
        var expiresAt = DateTime.UtcNow.AddMinutes(60); 

        return Ok(new LoginResponseDto(
        token,
        user.Email ?? string.Empty,
        user.FirstName ?? string.Empty,
        user.LastName ?? string.Empty,
        roles,
        expiresAt
        ));
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout successful. Please remove the token from client storage." });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserProfileDto(
        user.Id,
        user.Email ?? string.Empty,
        user.FirstName ?? string.Empty,
        user.LastName ?? string.Empty,
        user.RegisteredAt,
        roles
        ));
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(RegisterDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { message = "Profile updated successfully" });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { message = "Password changed successfully" });
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
         return NotFound(new { message = "User not found" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return Ok(new LoginResponseDto(
        token,
        user.Email ?? string.Empty,
        user.FirstName ?? string.Empty,
        user.LastName ?? string.Empty,
        roles,
        expiresAt
        ));
    }
}