using DoctorSystem.Data;
using DoctorSystem.DTOs;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services.Implementations
{
    public class TestResultService : ITestResultService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public TestResultService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<TestResult> CreateResultAsync(CreateTestResultDto dto, string doctorId)
        {
            var testRequest = await _context.TestRequests
                .Include(tr => tr.Patient)
                .Include(tr => tr.Doctor)
                .FirstOrDefaultAsync(tr => tr.Id == dto.TestRequestId && tr.DoctorId == doctorId);

            if (testRequest == null)
                throw new ArgumentException("Test request not found or unauthorized");

            var testResult = new TestResult
            {
                TestRequestId = dto.TestRequestId,
                ResultSummary = dto.ResultSummary,
                Notes = dto.Notes,
                SubmittedAt = DateTime.UtcNow
            };

            if (dto.ReportFile != null)
            {
                // Handle file upload logic here
                // Save file and set ReportFilePath
            }

            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();

            // Send email notification to doctor
            if (testRequest.Doctor != null)
            {
                await _emailService.SendEmailAsync(
                    testRequest.Doctor.Email,
                    "New Test Result Submitted",
                    $"Test result for patient {testRequest.Patient?.FullName} has been submitted. Please review it."
                );
            }

            // Send email notification to patient
            if (testRequest.Patient != null)
            {
                await _emailService.SendEmailAsync(
                    testRequest.Patient.Email,
                    "Your Test Result is Ready",
                    $"Dear {testRequest.Patient.FullName}, your test result is now available. Please log in to view it."
                );
            }

            return testResult;
        }

        public async Task<TestResult?> GetResultByRequestIdAsync(int requestId)
        {
            return await _context.TestResults
                .Include(tr => tr.TestRequest)
                    .ThenInclude(tr => tr.Patient)
                .Include(tr => tr.TestRequest)
                    .ThenInclude(tr => tr.Doctor)
                .FirstOrDefaultAsync(tr => tr.TestRequestId == requestId);
        }

        public async Task<List<TestResult>> GetResultsForPatientAsync(string patientId)
        {
            return await _context.TestResults
                .Include(tr => tr.TestRequest)
                    .ThenInclude(tr => tr.Doctor)
                .Where(tr => tr.TestRequest.PatientId == patientId)
                .OrderByDescending(tr => tr.SubmittedAt)
                .ToListAsync();
        }
    }
} 