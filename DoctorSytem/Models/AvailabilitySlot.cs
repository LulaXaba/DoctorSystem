// File: Models/AvailabilitySlot.cs
namespace DoctorSystem.Models
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }

        public string? ProviderId { get; set; }
        public ApplicationUser? Provider { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
    }
}
