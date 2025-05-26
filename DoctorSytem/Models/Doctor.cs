namespace DoctorSystem.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? ImagePath { get; set; }

        public List<AvailabilitySlot>? AvailabilitySlots { get; set; }
    }
}
