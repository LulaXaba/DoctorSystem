using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientResultsController : Controller
    {
        private readonly ITestResultService _testResultService;

        public PatientResultsController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var results = await _testResultService.GetResultsForPatientAsync(userId);
            return View(results);
        }
    }
} 