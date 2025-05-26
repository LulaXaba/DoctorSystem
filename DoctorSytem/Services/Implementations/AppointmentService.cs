using Microsoft.EntityFrameworkCore;
using DoctorSystem.Data;
using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DoctorSystem.Services.Implementations
{
    /// <summary>
    /// Implementation of IAppointmentService.
    /// Handles all appointment-related business logic and database operations.
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Creates appointments for multiple doctors.
        /// </summary>
        /// <param name="dto">Appointment creation data</param>
        /// <param name="patientId">ID of the patient booking the appointment</param>
        /// <returns>List of created appointment IDs</returns>
        public async Task<List<int>> CreateAppointmentAsync(CreateAppointmentDto dto, string patientId)
        {
            var appointments = new List<Appointment>();
            var startDateTime = dto.AppointmentDate.Date.Add(dto.StartTime);
            var endDateTime = dto.AppointmentDate.Date.Add(dto.EndTime);

            foreach (var doctorId in dto.DoctorIds)
            {
                var appointment = new Appointment
                {
                    PatientId = patientId,
                    DoctorId = doctorId,
                    Department = dto.Department,
                    StartTime = startDateTime,
                    EndTime = endDateTime,
                    Status = AppointmentStatus.Scheduled,
                    Notes = dto.Notes
                };
                appointments.Add(appointment);
            }

            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();

            // Log the creation of each appointment
            foreach (var appointment in appointments)
            {
                await LogAppointmentActionAsync(
                    appointment.Id,
                    patientId,
                    AuditActionType.Created,
                    null,
                    AppointmentStatus.Scheduled,
                    "Appointment created"
                );
            }

            return appointments.Select(a => a.Id).ToList();
        }

        /// <summary>
        /// Retrieves all appointments for a specific patient.
        /// </summary>
        /// <param name="patientId">ID of the patient</param>
        /// <returns>List of patient's appointments</returns>
        public async Task<List<Appointment>> GetPatientAppointmentsAsync(string patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific appointment by ID.
        /// </summary>
        /// <param name="appointmentId">ID of the appointment</param>
        /// <returns>The appointment if found, null otherwise</returns>
        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

        /// <summary>
        /// Cancels an appointment.
        /// </summary>
        /// <param name="dto">Cancellation data including appointment ID and optional reason</param>
        /// <param name="userId">ID of the user requesting cancellation</param>
        /// <returns>True if cancellation was successful, false otherwise</returns>
        public async Task<bool> CancelAppointmentAsync(CancelAppointmentDto dto, string userId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && 
                    (a.PatientId == userId || a.DoctorId == userId));

            if (appointment == null) return false;

            var previousStatus = appointment.Status;
            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancellationReason = dto.Reason;

            // Log the cancellation
            await LogAppointmentActionAsync(
                appointment.Id,
                userId,
                AuditActionType.Cancelled,
                previousStatus,
                AppointmentStatus.Cancelled,
                dto.Reason
            );

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Logs an appointment-related action for audit purposes.
        /// </summary>
        private async Task LogAppointmentActionAsync(
            int appointmentId,
            string userId,
            AuditActionType actionType,
            AppointmentStatus? previousStatus,
            AppointmentStatus? newStatus,
            string? details)
        {
            var log = new AppointmentAuditLog
            {
                AppointmentId = appointmentId,
                UserId = userId,
                ActionType = actionType,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                Details = details,
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
            };

            await _context.AppointmentAuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
} 