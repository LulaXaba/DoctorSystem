using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required, MaxLength(100)]
        public string MedicationName { get; set; }

        [Required, MaxLength(500)]
        public string DosageInstructions { get; set; }
    }
} 