using DoctorSystem.DTOs;
using DoctorSystem.Models;

namespace DoctorSystem.Services
{
    public interface ITestResultService
    {
        Task<TestResult> CreateResultAsync(CreateTestResultDto dto, string doctorId);
        Task<TestResult?> GetResultByRequestIdAsync(int requestId);
        Task<List<TestResult>> GetResultsForPatientAsync(string patientId);
    }
} 