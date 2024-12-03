using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitalMetrics.Models;
using VitalMetrics.Data;

namespace VitalMetrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccoountController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public AccoountController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Initiate Google Sign-In
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            // Authenticate the user via cookies
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest("Google authentication failed.");

            // Retrieve claims from Google
            var claims = authenticateResult.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var name = claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var googleId = claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            // Check if user already exists in the database
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                // Create a new user
                var user = new User
                {
                    FirstName = name,
                    Email = email,
                    GoogleId = googleId
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Optional: Update existing user info (e.g., name or GoogleId) if needed
                existingUser.FirstName = name;
                existingUser.GoogleId = googleId;
                _dbContext.Users.Update(existingUser);
                await _dbContext.SaveChangesAsync();
            }

            // Redirect to the requested page
            return LocalRedirect(returnUrl);
        }


        // Logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}