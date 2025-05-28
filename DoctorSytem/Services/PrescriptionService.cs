using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorSystem.Data;
using DoctorSystem.DTOs;
using DoctorSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public PrescriptionService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Prescription> CreateAsync(CreatePrescriptionDto dto, string doctorId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId && a.DoctorId == doctorId);

            if (appointment == null)
                throw new InvalidOperationException("Appointment not found or you don't have permission to create a prescription for it.");

            var prescription = new Prescription
            {
                AppointmentId = dto.AppointmentId,
                MedicationName = dto.MedicationName,
                DosageInstructions = dto.DosageInstructions,
                IssuedAt = DateTime.UtcNow
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            // Send email notification
            await _emailService.SendEmailAsync(
                appointment.Patient.Email,
                "New Prescription Issued",
                $"Your doctor has issued a new prescription for {dto.MedicationName}.\n\n" +
                $"Dosage Instructions: {dto.DosageInstructions}\n\n" +
                "Please log in to your account to view the complete prescription details."
            );

            return prescription;
        }

        public async Task<List<Prescription>> GetPrescriptionsForPatientAsync(string patientId)
        {
            return await _context.Prescriptions
                .Include(p => p.Appointment)
                .Where(p => p.Appointment.PatientId == patientId)
                .OrderByDescending(p => p.IssuedAt)
                .ToListAsync();
        }
    }
} 