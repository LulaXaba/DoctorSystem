using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs.Appointments
{
    /// <summary>
    /// Data Transfer Object for cancelling an appointment.
    /// </summary>
    public class CancelAppointmentDto
    {
        /// <summary>
        /// ID of the appointment to cancel
        /// </summary>
        [Required]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Optional reason for cancellation
        /// </summary>
        [MaxLength(500)]
        public string? Reason { get; set; }
    }
} 