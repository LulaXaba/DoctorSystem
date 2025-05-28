using System;
using System.ComponentModel.DataAnnotations;
using DoctorSystem.Data;

namespace DoctorSystem.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string DoctorId { get; set; }

        [Required]
        public string PatientId { get; set; }

        [Required, MaxLength(100)]
        public string MedicationName { get; set; }

        [Required, MaxLength(500)]
        public string DosageInstructions { get; set; }

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Appointment Appointment { get; set; }
        public ApplicationUser Doctor { get; set; }
        public ApplicationUser Patient { get; set; }
    }
}
