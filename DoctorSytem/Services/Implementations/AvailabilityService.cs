using DoctorSystem.Data;
using DoctorSystem.DTOs.Availability;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services.Implementations
{
    /// <summary>
    /// Service implementation for managing doctor availability slots.
    /// </summary>
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new availability slot for a doctor.
        /// </summary>
        public async Task<int> CreateAvailabilitySlotAsync(CreateAvailabilitySlotDto dto, string doctorId)
        {
            var slot = new AvailabilitySlot
            {
                DoctorId = doctorId,
                IsRecurring = dto.IsRecurring,
                DayOfWeek = dto.DayOfWeek,
                SpecificDate = dto.SpecificDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Notes = dto.Notes,
                IsActive = true
            };

            await _context.AvailabilitySlots.AddAsync(slot);
            await _context.SaveChangesAsync();
            return slot.Id;
        }

        /// <summary>
        /// Updates an existing availability slot.
        /// </summary>
        public async Task<bool> UpdateAvailabilitySlotAsync(UpdateAvailabilitySlotDto dto, string doctorId)
        {
            var slot = await _context.AvailabilitySlots
                .FirstOrDefaultAsync(s => s.Id == dto.Id && s.DoctorId == doctorId);

            if (slot == null)
                return false;

            slot.DayOfWeek = dto.DayOfWeek;
            slot.StartTime = dto.StartTime;
            slot.EndTime = dto.EndTime;
            slot.IsRecurring = dto.IsRecurring;
            slot.SpecificDate = dto.SpecificDate;
            slot.IsActive = dto.IsActive;
            slot.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes an availability slot.
        /// </summary>
        public async Task<bool> DeleteAvailabilitySlotAsync(int slotId, string doctorId)
        {
            var slot = await _context.AvailabilitySlots
                .FirstOrDefaultAsync(s => s.Id == slotId && s.DoctorId == doctorId);

            if (slot == null)
                return false;

            _context.AvailabilitySlots.Remove(slot);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets all availability slots for a doctor.
        /// </summary>
        public async Task<List<AvailabilitySlot>> GetDoctorAvailabilityAsync(string doctorId)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific availability slot.
        /// </summary>
        public async Task<AvailabilitySlot?> GetAvailabilitySlotAsync(int slotId)
        {
            return await _context.AvailabilitySlots
                .FirstOrDefaultAsync(s => s.Id == slotId);
        }

        /// <summary>
        /// Gets all active availability slots for a doctor on a specific date.
        /// </summary>
        public async Task<List<AvailabilitySlot>> GetDoctorAvailabilityForDateAsync(string doctorId, DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            
            return await _context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId && s.IsActive &&
                    ((s.IsRecurring && s.DayOfWeek == dayOfWeek) ||
                     (!s.IsRecurring && s.SpecificDate == date)))
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Checks if a doctor is available at a specific time.
        /// </summary>
        public async Task<bool> IsDoctorAvailableAsync(string doctorId, DateTime startTime, DateTime endTime)
        {
            var dayOfWeek = startTime.DayOfWeek;
            var date = startTime.Date;

            // Check for any availability slots that overlap with the requested time
            var hasAvailability = await _context.AvailabilitySlots
                .AnyAsync(s => s.DoctorId == doctorId && s.IsActive &&
                    ((s.IsRecurring && s.DayOfWeek == dayOfWeek) ||
                     (!s.IsRecurring && s.SpecificDate == date)) &&
                    s.StartTime <= startTime.TimeOfDay &&
                    s.EndTime >= endTime.TimeOfDay);

            if (!hasAvailability)
                return false;

            // Check for any existing appointments that overlap with the requested time
            var hasOverlappingAppointment = await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                    a.Status == AppointmentStatus.Scheduled &&
                    a.StartTime < endTime &&
                    a.EndTime > startTime);

            return !hasOverlappingAppointment;
        }
    }
} 