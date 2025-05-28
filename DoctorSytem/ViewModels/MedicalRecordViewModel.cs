using System.Collections.Generic;
using DoctorSystem.Models;

namespace DoctorSystem.ViewModels
{
    public class MedicalRecordViewModel
    {
        public List<Appointment> Appointments { get; set; }
        public List<TestResult> TestResults { get; set; }
        public List<Prescription> Prescriptions { get; set; }
    }
} 