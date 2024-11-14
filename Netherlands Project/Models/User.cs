namespace Netherlands_Project.Models
{
    public class User : Common
    {
       
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
    public class SignUpDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class LoginDTO
    { 
        public string? Email { get; set; }
        public string? Password { get; set; }
        
    }
}
