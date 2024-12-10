using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using VitalMetrics.Models;

namespace VitalMetrics.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Brug et regex-mønster til at validere e-mail-formatet
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        private string GenerateJwtToken(User user)
        {


            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (_configuration["Authentication:JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Authentication:JwtSettings:Issuer"],
                _configuration["Authentication:JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
