using DoctorSystem.DTOs;
using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(CreatePaymentDto dto, string patientId);
        Task<IEnumerable<Payment>> GetPatientPaymentsAsync(string patientId);
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
    }
} 