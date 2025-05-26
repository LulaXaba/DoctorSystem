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

        // FK to doctor/provider
        public string ProviderId { get; set; } = null!;

        [ForeignKey("ProviderId")]
        public ApplicationUser? Provider { get; set; }

        // FK to customer (user who booked)
        public string CustomerId { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public ApplicationUser? Customer { get; set; }

        public string Department { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? CancellationReason { get; set; }

        public string PatientId { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
