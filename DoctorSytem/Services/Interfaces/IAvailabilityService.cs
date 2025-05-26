using DoctorSystem.DTOs.Availability;
using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<AvailabilitySlot>> GetDoctorAvailabilityAsync(string doctorId);
        Task<AvailabilitySlot?> GetAvailabilitySlotAsync(int slotId);
        Task<List<AvailabilitySlot>> GetDoctorAvailabilityForDateAsync(string doctorId, DateTime date);
        Task<bool> IsDoctorAvailableAsync(string doctorId, DateTime startTime, DateTime endTime);
        Task<int> CreateAvailabilitySlotAsync(CreateAvailabilitySlotDto dto, string doctorId);
        Task<bool> UpdateAvailabilitySlotAsync(UpdateAvailabilitySlotDto dto, string doctorId);
        Task<bool> DeleteAvailabilitySlotAsync(int slotId, string doctorId);
    }
} 