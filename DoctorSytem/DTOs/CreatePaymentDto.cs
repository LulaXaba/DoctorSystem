using System.ComponentModel.DataAnnotations;
using DoctorSystem.Models;

namespace DoctorSystem.DTOs
{
    public class CreatePaymentDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        public string? ReferenceNumber { get; set; }

        public int? AppointmentId { get; set; }
    }
} 