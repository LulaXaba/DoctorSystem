using DoctorSystem.Data;
using DoctorSystem.DTOs;
using DoctorSystem.Models;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Services.Interfaces;

namespace DoctorSystem.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        public TestResultService(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _context = context;
            _environment = environment;
            _emailService = emailService;
        }

        public async Task<TestResult> CreateResultAsync(CreateTestResultDto dto, string doctorId)
        {
            var request = await _context.TestRequests
                .Include(tr => tr.Result)
                .Include(tr => tr.Patient)
                .FirstOrDefaultAsync(tr => tr.Id == dto.TestRequestId && tr.DoctorId == doctorId);

            if (request == null)
                throw new InvalidOperationException("Test request not found or you don't have permission to submit results for it.");

            if (request.Result != null)
                throw new InvalidOperationException("Results have already been submitted for this test request.");

            var result = new TestResult
            {
                TestRequestId = dto.TestRequestId,
                ResultSummary = dto.ResultSummary,
                Notes = dto.Notes,
                SubmittedAt = DateTime.UtcNow
            };

            if (dto.ReportFile != null && dto.ReportFile.Length > 0)
            {
                // Validate file type
                if (!dto.ReportFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Only PDF files are allowed.");

                // Validate file size (max 10MB)
                if (dto.ReportFile.Length > 10 * 1024 * 1024)
                    throw new InvalidOperationException("File size must be less than 10MB.");

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "test-reports");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.ReportFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ReportFile.CopyToAsync(stream);
                }

                result.ReportFilePath = $"/uploads/test-reports/{fileName}";
            }

            _context.TestResults.Add(result);
            request.Status = "Completed";

            await _context.SaveChangesAsync();

            // Send email notification
            if (request.Patient != null)
            {
                try
                {
                    await _emailService.SendTestResultNotificationAsync(
                        request.Patient.Email,
                        request.Patient.FullName,
                        request.TestType,
                        dto.ResultSummary
                    );
                }
                catch (Exception ex)
                {
                    // Log the email error but don't fail the operation
                    // TODO: Add proper logging
                    Console.WriteLine($"Failed to send email notification: {ex.Message}");
                }
            }

            return result;
        }

        public async Task<TestResult?> GetResultByRequestIdAsync(int requestId)
        {
            return await _context.TestResults
                .Include(r => r.TestRequest)
                .FirstOrDefaultAsync(r => r.TestRequestId == requestId);
        }

        public async Task<List<TestResult>> GetResultsForPatientAsync(string patientId)
        {
            return await _context.TestResults
                .Include(r => r.TestRequest)
                .Where(r => r.TestRequest.PatientId == patientId)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();
        }
    }
} 