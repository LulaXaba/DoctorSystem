namespace DoctorSystem.DTOs.Availability
{
    public class CreateAvailabilitySlotDto
    {
        public bool IsRecurring { get; set; }
        public DayOfWeek? DayOfWeek { get; set; }
        public DateTime? SpecificDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Notes { get; set; }
    }
} 