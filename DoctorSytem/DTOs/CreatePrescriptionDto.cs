using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Medication Name")]
        public string MedicationName { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Dosage Instructions")]
        public string DosageInstructions { get; set; }
    }
} 