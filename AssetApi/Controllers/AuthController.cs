using AssetApi.Data;
using AssetApi.DTOs;
using AssetApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController(AppDbContext db, ITokenService tokenService) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and returns a JWT bearer token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await db.Users
            .SingleOrDefaultAsync(u => u.Username == request.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid username or password." });

        var (token, expiresAt) = tokenService.GenerateToken(user);

        return Ok(new LoginResponse(token, expiresAt, user.Username, user.Role));
    }
}
