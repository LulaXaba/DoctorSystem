using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointmentAsync(CreateAppointmentDto dto, string patientId);
        Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(string patientId);
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<bool> CancelAppointmentAsync(int id, string userId);
    }
} 