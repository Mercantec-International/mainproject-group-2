using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VitalMetrics.Data;
using VitalMetrics.Models;
using Microsoft.EntityFrameworkCore;

namespace VitalMetrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AuthController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/api/auth/google-callback" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return Unauthorized();

            var claims = authenticateResult.Principal.Claims;

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var profilePicture = claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            if (email == null || googleId == null)
                return BadRequest("Google authentication failed.");

            // Check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
            if (user == null)
            {
                // Register new user
                user = new User
                {
                    Username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                    Email = email,
                    GoogleId = googleId,
                    ProfilePicture = profilePicture
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Generate your token or set user identity here
            return Ok(new { message = "Google Sign-In successful", user });
        }
    }
}