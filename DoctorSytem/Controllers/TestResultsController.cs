using DoctorSystem.DTOs;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class TestResultsController : Controller
    {
        private readonly ITestResultService _testResultService;

        public TestResultsController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        public IActionResult Create(int requestId)
        {
            var model = new CreateTestResultDto { TestRequestId = requestId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTestResultDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
                return View(dto);
            }

            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(doctorId))
            {
                TempData["ErrorMessage"] = "Unable to identify the current user.";
                return View(dto);
            }

            try
            {
                await _testResultService.CreateResultAsync(dto, doctorId);
                TempData["SuccessMessage"] = "Test result has been successfully submitted.";
                return RedirectToAction("Index", "TestRequests");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(dto);
            }
        }
    }
} 