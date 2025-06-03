using DoctorSystem.Data;
using DoctorSystem.DTOs;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public PaymentService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Payment> CreatePaymentAsync(CreatePaymentDto dto, string patientId)
        {
            if (dto.AppointmentId.HasValue)
            {
                var appointmentExists = await _context.Appointments.AnyAsync(a => a.Id == dto.AppointmentId.Value);
                if (!appointmentExists)
                {
                    throw new InvalidOperationException("The specified appointment does not exist.");
                }
            }

            var payment = new Payment
            {
                PatientId = patientId,
                Amount = dto.Amount,
                Method = dto.Method,
                ReferenceNumber = dto.ReferenceNumber,
                AppointmentId = dto.AppointmentId,
                Status = PaymentStatus.Pending
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // If payment is successful (e.g., PayPal), send confirmation email
            if (dto.Method == PaymentMethod.PayPal)
            {
                payment.Status = PaymentStatus.Completed;
                await _context.SaveChangesAsync();
                await _emailService.SendPaymentConfirmationAsync(payment);
            }

            return payment;
        }

        public async Task<IEnumerable<Payment>> GetPatientPaymentsAsync(string patientId)
        {
            return await _context.Payments
                .Include(p => p.Appointment)
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found");

            payment.Status = status;
            await _context.SaveChangesAsync();

            if (status == PaymentStatus.Completed)
            {
                await _emailService.SendPaymentConfirmationAsync(payment);
            }
        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public IEnumerable<Appointment> GetPatientAppointments(string patientId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.StartTime)
                .ToList();
        }
    }
} 