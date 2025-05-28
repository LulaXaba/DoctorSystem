using System.Threading.Tasks;
using DoctorSystem.Data;
using DoctorSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalRecordViewModel> GetPatientMedicalRecordAsync(string patientId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            var testResults = await _context.TestResults
                .Include(t => t.TestRequest)
                .Where(t => t.TestRequest.PatientId == patientId)
                .OrderByDescending(t => t.SubmittedAt)
                .ToListAsync();

            var prescriptions = await _context.Prescriptions
                .Include(p => p.Appointment)
                .Where(p => p.Appointment.PatientId == patientId)
                .OrderByDescending(p => p.IssuedAt)
                .ToListAsync();

            return new MedicalRecordViewModel
            {
                Appointments = appointments,
                TestResults = testResults,
                Prescriptions = prescriptions
            };
        }
    }
}