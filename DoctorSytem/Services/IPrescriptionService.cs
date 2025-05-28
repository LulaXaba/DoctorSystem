using System.Collections.Generic;
using System.Threading.Tasks;
using DoctorSystem.DTOs;
using DoctorSystem.Models;

namespace DoctorSystem.Services
{
    public interface IPrescriptionService
    {
        Task<Prescription> CreateAsync(CreatePrescriptionDto dto, string doctorId);
        Task<List<Prescription>> GetPrescriptionsForPatientAsync(string patientId);
    }
} 