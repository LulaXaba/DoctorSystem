using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs.Availability
{
    /// <summary>
    /// Data Transfer Object for updating an existing availability slot.
    /// </summary>
    public class UpdateAvailabilitySlotDto
    {
        /// <summary>
        /// ID of the availability slot to update
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Day of the week for this availability slot
        /// </summary>
        [Required]
        public DayOfWeek? DayOfWeek { get; set; }

        /// <summary>
        /// Start time of the availability slot
        /// </summary>
        [Required]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// End time of the availability slot
        /// </summary>
        [Required]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Whether this slot is recurring weekly
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Specific date for non-recurring slots
        /// </summary>
        public DateTime? SpecificDate { get; set; }

        /// <summary>
        /// Whether this slot is currently active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Notes about this availability slot
        /// </summary>
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
} 