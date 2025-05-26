using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DoctorSystem.DTOs
{
    public class CreateTestResultDto
    {
        [Required]
        public int TestRequestId { get; set; }

        [Required]
        [StringLength(500)]
        public string ResultSummary { get; set; } = null!;

        [StringLength(1000)]
        public string? Notes { get; set; }

        public IFormFile? ReportFile { get; set; }
    }
} 