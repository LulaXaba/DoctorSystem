using Microsoft.AspNetCore.Identity;

namespace DoctorSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? MedicalHistoryFilePath { get; set; }
        public string? Role { get; set; } 
    }
}
