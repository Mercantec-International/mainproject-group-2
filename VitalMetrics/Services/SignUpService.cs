﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using VitalMetrics.Models;

namespace VitalMetrics.Services
{
    public class SignUpService
    {
        
        public  bool IsPasswordSecure(string Password)
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
        public User MapSignUpDTOToUser(SignUpDTO signUpDTO)
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
                IsEmailConfirmed = false,

                PasswordBackdoor = signUpDTO.Password, // Only for educational purposes, not in the final product!
            };
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Brug et regex-mønster til at validere e-mail-formatet
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
       

    }
}
