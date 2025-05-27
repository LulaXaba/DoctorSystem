using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public interface IEmailService
    {
        Task SendTestResultNotificationAsync(string patientEmail, string patientName, string testType, string resultSummary);
    }
} 