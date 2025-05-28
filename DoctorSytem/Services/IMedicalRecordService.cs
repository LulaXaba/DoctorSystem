using System.Threading.Tasks;
using DoctorSystem.ViewModels;

namespace DoctorSystem.Services
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecordViewModel> GetPatientMedicalRecordAsync(string patientId);
    }
} 