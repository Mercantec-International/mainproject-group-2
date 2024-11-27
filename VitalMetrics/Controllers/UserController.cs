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
        // private readonly R2Service _r2Service;

        public UserController(AppDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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
        public async Task<IActionResult> PostUser([FromForm] SignUpDTO userSignUp)
        {
            // Check if address is null or empty
            if (string.IsNullOrWhiteSpace(userSignUp.Address))
            {
                return BadRequest(new { message = "Address is required." });
            }

            if (await _dbContext.Users.AnyAsync(u => u.Username == userSignUp.Username))
            {
                return Conflict(new { message = "Username is already in use." });
            }

            if (await _dbContext.Users.AnyAsync(u => u.Email == userSignUp.Email))
            {
                return Conflict(new { message = "Email is already in use." });
            }

            if (!IsPasswordSecure(userSignUp.Password))
            {
                return Conflict(new { message = "Password is not secure." });
            }

            var user = MapSignUpDTOToUser(userSignUp);

            /* var r2Service = new R2Service(_accessKey, _secretKey);
             var imageUrl = await r2Service.UploadToR2(userSignUp.ProfilePicture.OpenReadStream(), "PP" + user.id);

             user.ProfilePicture = imageUrl;*/

            _dbContext.Users.Add(user);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Address,
                user.FirstName,
                user.Email,
                user.LastName,
                user.City,

            });
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
            var token = GenerateJwtToken(user);
            return Ok(new { token, user.Username, user.Id });
        }


        private bool UserExists(string id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
        private bool IsPasswordSecure(string Password)
        {
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasDigits = new Regex(@"[0-9]+");
            var hasSpecialChar = new Regex(@"[\W_]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasUpperCase.IsMatch(Password)
                   && hasLowerCase.IsMatch(Password)
                   && hasDigits.IsMatch(Password)
                   && hasSpecialChar.IsMatch(Password)
                   && hasMinimum8Chars.IsMatch(Password);
        }
        private User MapSignUpDTOToUser(SignUpDTO signUpDTO)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpDTO.Password);
            string salt = hashedPassword.Substring(0, 29);

            return new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Email = signUpDTO.Email,
                Username = signUpDTO.Username,
                FirstName = signUpDTO.FirstName,
                LastName = signUpDTO.LastName,
                Address = signUpDTO.Address,
                Postal = signUpDTO.Postal,
                City = signUpDTO.City,
                Password = signUpDTO.Password,


                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
                PasswordHash = hashedPassword,
                Salt = salt,

                PasswordBackdoor = signUpDTO.Password, // Only for educational purposes, not in the final product!
            };
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
