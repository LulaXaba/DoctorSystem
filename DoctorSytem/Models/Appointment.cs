using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }

    public class Appointment
    {
        public int Id { get; set; }

        // FK to doctor
        public string DoctorId { get; set; } = null!;

        [ForeignKey("DoctorId")]
        public ApplicationUser? Doctor { get; set; }

        // FK to patient
        public string PatientId { get; set; } = null!;

        [ForeignKey("PatientId")]
        public ApplicationUser? Patient { get; set; }

        public string Department { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? CancellationReason { get; set; }
        
        // Check-in related fields
        public DateTime? CheckedInAt { get; set; }
        public string? CheckInNotes { get; set; }
    }
}
