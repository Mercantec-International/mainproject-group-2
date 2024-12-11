using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using VitalMetrics.Data;
using VitalMetrics.Models;
using VitalMetrics.Services;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalMetrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly SignUpService _signupService;
        private readonly EmailService _emailService;
        private readonly JWTService _jwtService;


        // private readonly R2Service _r2Service;

        public UserController
            (
            AppDBContext dbContext, 
            IConfiguration configuration,
            SignUpService signupService,
            JWTService jwtService
,
            EmailService emailService
            )
        {
            _signupService = signupService;
            _dbContext = dbContext;
            _configuration = configuration;
            _emailService = emailService;
            _jwtService = jwtService;
            //_accessKey = config.AccessKey;
            //_secretKey = config.SecretKey;
            //_r2Service = new R2Service(_accessKey, _secretKey);
        }
        // get all 
        // GET api/<UserController>/5
        [HttpGet("getuserbyid/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        // create user
        [HttpPost("SignUp")]
        public async Task<IActionResult> PostUser( SignUpDTO userSignUp)
        {


            if (!_signupService.IsValidEmail(userSignUp.Email))
            {
                return BadRequest(new { message = "Ugyldig e-mailadresse." });
            }

            if (!_signupService.IsPasswordSecure(userSignUp.Password))
            {
                return Conflict(new { message = "Adgangskoden er ikke sikker nok." });
            }

            if (await _dbContext.Users.AnyAsync(u => u.Email == userSignUp.Email))
            {
                return Conflict(new { message = "E-mailadressen er allerede i brug." });
            }

            var user = _signupService.MapSignUpDTOToUser(userSignUp);

            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            user.IsEmailConfirmed = false;

            _dbContext.Users.Add(user);

            try
            {
                await _dbContext.SaveChangesAsync();

                await _emailService.SendConfirmationEmail(user.Email, user.EmailConfirmationToken);

                return Ok(
                    new
                    {
                        user.Id,
                        user.Email,
                        message = "Bruger oprettet. Tjek venligst din email for at bekræfte din konto."
                    }
                );
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                throw;
            }
        }
        private bool UserExists(string id)
        {
            return _dbContext.Users.Any(e => e.Id == id);   
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            if (!user.IsEmailConfirmed)
            {
                return Unauthorized(
                    new
                    {
                        message = "Email er ikke bekræftet. Tjek venligst din email for bekræftelses-link."
                    }
                );
            }
            var (accessToken, refreshToken) = _jwtService.GenerateTokens(user);

            return Ok(
               new
               {
                   accessToken,
                   refreshToken,
                  
               }
           );
        }


        // Tilføj nyt endpoint til email bekræftelse
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
            [FromQuery] string token,
            [FromQuery] string email
        )
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u =>
                u.Email == email && u.EmailConfirmationToken == token
            );

            var baseUrl =
                Environment.GetEnvironmentVariable("APPLICATION_BASE_URL")
                ?? _configuration["Application:BaseUrl"];

            if (user == null)
            {
                return Redirect($"https://{baseUrl}/email-confirmation?status=error");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            await _dbContext.SaveChangesAsync();

            return Redirect($"https://{baseUrl}/email-confirmation?status=success");
        }




        // DELETE api/<UserController>/5
        [HttpDelete("deleteuserbyid/{id}")]
        public void Delete(int id)
        {
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
