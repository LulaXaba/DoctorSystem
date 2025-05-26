using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    /// <summary>
    /// Represents an audit log entry for appointment-related actions.
    /// </summary>
    public class AppointmentAuditLog
    {
        public int Id { get; set; }

        /// <summary>
        /// ID of the appointment being audited
        /// </summary>
        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }

        /// <summary>
        /// ID of the user who performed the action
        /// </summary>
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Type of action performed
        /// </summary>
        [Required]
        public AuditActionType ActionType { get; set; }

        /// <summary>
        /// Previous status of the appointment (if applicable)
        /// </summary>
        public AppointmentStatus? PreviousStatus { get; set; }

        /// <summary>
        /// New status of the appointment (if applicable)
        /// </summary>
        public AppointmentStatus? NewStatus { get; set; }

        /// <summary>
        /// Additional details about the action
        /// </summary>
        [MaxLength(1000)]
        public string? Details { get; set; }

        /// <summary>
        /// Timestamp when the action was performed
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IP address of the user who performed the action
        /// </summary>
        [MaxLength(50)]
        public string? IpAddress { get; set; }
    }

    /// <summary>
    /// Types of actions that can be audited
    /// </summary>
    public enum AuditActionType
    {
        Created,
        Cancelled,
        Rescheduled,
        Completed,
        Modified
    }
} 