using DoctorSystem.Models;

namespace DoctorSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAppointmentConfirmationAsync(Appointment appointment);
        Task SendTestRequestConfirmationAsync(TestRequest request);
        Task SendPrescriptionConfirmationAsync(Prescription prescription);
        Task SendPaymentConfirmationAsync(Payment payment);
        Task SendTestResultConfirmationAsync(TestResult result);
        Task SendEmailAsync(string to, string subject, string body);
        Task SendTestResultNotificationAsync(string patientEmail, string patientName, string testType, string resultSummary);
    }
} 