using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Models.ViewModels
{
    /// <summary>
    /// View model for appointment cancellation.
    /// </summary>
    public class CancelAppointmentViewModel
    {
        /// <summary>
        /// ID of the appointment to cancel
        /// </summary>
        [Required]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Optional reason for cancellation
        /// </summary>
        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        public string? Reason { get; set; }

        /// <summary>
        /// Appointment details for display
        /// </summary>
        public Appointment? Appointment { get; set; }
    }
} 