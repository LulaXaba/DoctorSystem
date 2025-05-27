using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DoctorSystem.Models;

namespace DoctorSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendTestResultNotificationAsync(string patientEmail, string patientName, string testType, string resultSummary)
        {
            using var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };

            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, "Doctor System"),
                Subject = $"New Test Result Available: {testType}",
                Body = $@"
                    <h2>New Test Result Available</h2>
                    <p>Dear {patientName},</p>
                    <p>Your test result for {testType} is now available.</p>
                    <p><strong>Result Summary:</strong></p>
                    <p>{resultSummary}</p>
                    <p>Please log in to your account to view the complete test result and any attached documents.</p>
                    <p>Best regards,<br>Doctor System Team</p>",
                IsBodyHtml = true
            };

            message.To.Add(patientEmail);

            await client.SendMailAsync(message);
        }
    }
} 