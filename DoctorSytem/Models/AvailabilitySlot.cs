﻿// File: Models/AvailabilitySlot.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    /// <summary>
    /// Represents a time slot when a doctor is available for appointments.
    /// </summary>
    public class AvailabilitySlot
    {
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the doctor
        /// </summary>
        [Required]
        public string DoctorId { get; set; } = null!;

        [ForeignKey("DoctorId")]
        public ApplicationUser? Doctor { get; set; }

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
        public bool IsRecurring { get; set; } = true;

        /// <summary>
        /// Specific date for non-recurring slots
        /// </summary>
        public DateTime? SpecificDate { get; set; }

        /// <summary>
        /// Whether this slot is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Notes about this availability slot
        /// </summary>
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
