using DoctorSystem.Data;
using DoctorSystem.DTOs;
using DoctorSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorSystem.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ApplicationDbContext _context;

        public TestResultService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TestResult> CreateResultAsync(CreateTestResultDto dto, string doctorId)
        {
            var request = await _context.TestRequests
                .Include(tr => tr.Result)
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

            _context.TestResults.Add(result);
            request.Status = "Completed";

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<TestResult?> GetResultByRequestIdAsync(int requestId)
        {
            return await _context.TestResults
                .Include(r => r.TestRequest)
                .FirstOrDefaultAsync(r => r.TestRequestId == requestId);
        }
    }
} 