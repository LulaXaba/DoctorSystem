using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs.Appointments
{
    public class CheckInDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
} 