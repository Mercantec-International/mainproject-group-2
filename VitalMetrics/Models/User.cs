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
<<<<<<< HEAD
        public string? GoogleId { get; set; } // Google Account ID
=======
        public string? GoogleId { get; set; } 
>>>>>>> c91675d5846bf3f974754526e58dceaea92cb229
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordBackdoor { get; set; }
        public string? Salt { get; set; }



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
