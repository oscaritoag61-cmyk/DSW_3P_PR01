using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using DSW_3P_PR01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);
    private const string DemoUsername = "admin";
    private const string DemoPassword = "questlog123";
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenResponse> Login(LoginRequest request)
    {
        var username = Encoding.UTF8.GetBytes(request.Username);
        var password = Encoding.UTF8.GetBytes(request.Password);
        var demoUsername = Encoding.UTF8.GetBytes(DemoUsername);
        var demoPassword = Encoding.UTF8.GetBytes(DemoPassword);

        if (username.Length != demoUsername.Length
            || password.Length != demoPassword.Length
            || !CryptographicOperations.FixedTimeEquals(username, demoUsername)
            || !CryptographicOperations.FixedTimeEquals(password, demoPassword))
        {
            return Unauthorized();
        }

        return CreateTokenResponse();
    }

    [AllowAnonymous]
    [HttpPost("token")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<TokenResponse> CreateToken(TokenRequest request)
    {
        var configuredApiKey = Encoding.UTF8.GetBytes(_configuration["Authentication:ApiKey"]!);
        var providedApiKey = Encoding.UTF8.GetBytes(request.ApiKey);
        if (configuredApiKey.Length != providedApiKey.Length ||
            !CryptographicOperations.FixedTimeEquals(configuredApiKey, providedApiKey))
        {
            return Unauthorized();
        }

        return CreateTokenResponse();
    }

    private ActionResult<TokenResponse> CreateTokenResponse()
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.Add(TokenLifetime);
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Authentication:SigningKey"]!));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Authentication:Issuer"] ?? "DSW_3P_PR01",
            audience: _configuration["Authentication:Audience"] ?? "DSW_3P_PR01",
            notBefore: now,
            expires: expiresAt,
            signingCredentials: credentials);

        return Ok(new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt));
    }
}
