using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.ViewModels
{
    public class BookAppointmentViewModel
    {
        [Required]
        public string? Department { get; set; }

        [Required]
        public string? DoctorName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string? TimeSlot { get; set; }

        public List<string> Departments { get; set; } = new() { "Cardiology", "Pediatrics", "Dentistry", "General" };
        public List<string> TimeSlots { get; set; } = new() { "09:00 AM", "10:00 AM", "11:00 AM", "02:00 PM" };
    }
}
