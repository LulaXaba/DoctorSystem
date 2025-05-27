using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<int>> CreateAppointmentAsync(CreateAppointmentDto dto, string patientId);
        Task<List<Appointment>> GetPatientAppointmentsAsync(string patientId);
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task<bool> CancelAppointmentAsync(CancelAppointmentDto dto, string userId);
        Task<bool> CheckInAsync(CheckInDto dto, string patientId);
    }
} 