using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VitalMetrics.Models;
using VitalMetrics.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly JWTService _jwtService;
    public TokenService
        (
        JWTService jwtService,
        IConfiguration configuration
        )
    {
        _configuration = configuration;
        _jwtService = jwtService;
    }

    public string GenerateEmailConfirmationToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("Purpose", "EmailConfirmation")
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Token expiry (1 hour)
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
   
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Immediate expiry check
            }, out _);

            return principal;
        }
        catch
        {
            throw new SecurityTokenException("Invalid token");
        }

    }
}
