using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        
        [Required]
        public int TestRequestId { get; set; }
        
        [ForeignKey("TestRequestId")]
        public TestRequest? TestRequest { get; set; }
        
        [Required]
        [StringLength(500)]
        public string ResultSummary { get; set; } = null!;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        public string? ReportFilePath { get; set; }
    }
} 