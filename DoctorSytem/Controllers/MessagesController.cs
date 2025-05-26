using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DoctorSystem.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    public class MessagesController : Controller
    {
        private readonly SmtpSettings _smtp;

        public MessagesController(IOptions<SmtpSettings> smtpOptions)
        {
            _smtp = smtpOptions.Value;
        }

        [HttpGet]
        public IActionResult Compose()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send(string toEmail, string subject, string body)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_smtp.SenderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                using var smtp = new SmtpClient(_smtp.Host, _smtp.Port)
                {
                    Credentials = new NetworkCredential(_smtp.SenderEmail, _smtp.SenderPassword),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail);
                TempData["Success"] = "✅ Email sent successfully!";
            }
            catch
            {
                TempData["Error"] = "❌ Failed to send the email. Please check configuration or try again.";
            }

            return RedirectToAction("Compose");
        }
    }
}
