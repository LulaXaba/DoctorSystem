using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs.Appointments
{
    /// <summary>
    /// Data Transfer Object for creating a new appointment.
    /// Contains all necessary information to create an appointment in the system.
    /// </summary>
    public class CreateAppointmentDto
    {
        /// <summary>
        /// List of selected doctor IDs (maximum 3)
        /// </summary>
        [Required]
        public List<string> DoctorIds { get; set; } = new();

        /// <summary>
        /// Department for the appointment
        /// </summary>
        [Required]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Date of the appointment
        /// </summary>
        [Required]
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Start time of the appointment
        /// </summary>
        [Required]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End time of the appointment
        /// </summary>
        [Required]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Optional notes for the appointment
        /// </summary>
        public string? Notes { get; set; }
    }
} 