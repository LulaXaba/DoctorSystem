using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    public class TestRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string PatientId { get; set; } = null!;
        
        [ForeignKey("PatientId")]
        public ApplicationUser? Patient { get; set; }
        
        [Required]
        public string DoctorId { get; set; } = null!;
        
        [ForeignKey("DoctorId")]
        public ApplicationUser? Doctor { get; set; }
        
        [Required]
        public string TestType { get; set; } = null!;
        
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        
        public string Status { get; set; } = "Pending"; // Pending, Completed
        
        public TestResult? Result { get; set; }
    }
} 