namespace VitalMetrics.Models
{
    public class User : Common
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Postal { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? GoogleId { get; set; } // Google Account ID
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordBackdoor { get; set; }
        public string? Salt { get; set; }
        public ICollection<Oxilevel> Oxilevels { get; set; }
        public ICollection<Accelerometer> Accelerometers { get; set; }
        public ICollection<FHeartbeat> FingerHeartbeats { get; set; }
    }

    public class SignUpDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Postal { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
    }

    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
