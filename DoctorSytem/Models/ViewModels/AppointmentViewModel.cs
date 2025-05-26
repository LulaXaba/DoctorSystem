using System.ComponentModel.DataAnnotations;
using DoctorSystem.Models;
using DoctorSystem.Validators;

namespace DoctorSystem.Models.ViewModels
{
    /// <summary>
    /// View model for appointment creation form.
    /// Handles data binding and validation for the appointment booking process.
    /// </summary>
    public class AppointmentViewModel
    {
        /// <summary>
        /// List of available doctors for selection
        /// </summary>
        public List<ApplicationUser> ProviderList { get; set; } = new();

        /// <summary>
        /// Available time slots for appointment scheduling (09:00-17:00)
        /// </summary>
        public List<TimeSpan> AvailableTimeSlots { get; set; } = new();

        /// <summary>
        /// Selected doctors (maximum 3)
        /// </summary>
        [Required(ErrorMessage = "Please select at least one doctor.")]
        [MaxLength(3, ErrorMessage = "You can select a maximum of 3 doctors.")]
        public List<string> SelectedDoctors { get; set; } = new();

        /// <summary>
        /// Department for the appointment
        /// </summary>
        [Required]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Date of the appointment
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Start time of the appointment (must be between 09:00-17:00)
        /// </summary>
        [Required]
        [TimeRange(9, 17, ErrorMessage = "Start time must be between 09:00 and 17:00")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End time of the appointment (must be between 09:00-17:00 and after start time)
        /// </summary>
        [Required]
        [TimeRange(9, 17, ErrorMessage = "End time must be between 09:00 and 17:00")]
        [TimeAfter("StartTime", ErrorMessage = "End time must be after start time")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Optional notes for the appointment
        /// </summary>
        public string? Description { get; set; }
    }
} 




