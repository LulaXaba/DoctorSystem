using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    public enum PaymentMethod { EFT, PayPal }
    public enum PaymentStatus { Pending, Completed, Failed }

    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public string PatientId { get; set; } = default!;
        
        [ForeignKey("PatientId")]
        public ApplicationUser Patient { get; set; } = default!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? ReferenceNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional FK
        public int? AppointmentId { get; set; }
        
        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }
    }
}
