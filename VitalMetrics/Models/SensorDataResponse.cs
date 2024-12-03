namespace VitalMetrics.Models
{
    public class SensorDataResponse
    {
        public int? OxygenLevel { get; set; }
        public int? HeartRateBPM { get; set; }
        public int? Changes { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Z { get; set; }
        public int? BPM { get; set; }
        public string? UserId { get; set; }
    }
}
