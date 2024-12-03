namespace VitalMetrics.Models
{
    public class Common 
    {
        public string? Id { get; set; } // Kan erstattes med "int Id"
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UserId { get; set; } // Foreign Key
    }
}
