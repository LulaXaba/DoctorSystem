using Microsoft.EntityFrameworkCore;
using DoctorSystem.Data;
using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;

namespace DoctorSystem.Services.Implementations
{
    /// <summary>
    /// Implementation of IAppointmentService.
    /// Handles all appointment-related business logic and database operations.
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
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
                .Include(a => a.Doctor)
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
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);
        }

        /// <summary>
        /// Cancels an appointment.
        /// </summary>
        /// <param name="appointmentId">ID of the appointment to cancel</param>
        /// <returns>True if cancellation was successful, false otherwise</returns>
        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 