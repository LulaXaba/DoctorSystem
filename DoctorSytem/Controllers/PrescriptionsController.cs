using System.Threading.Tasks;
using DoctorSystem.DTOs;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class PrescriptionsController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public IActionResult Create(int appointmentId)
        {
            ViewBag.AppointmentId = appointmentId;
            return View(new CreatePrescriptionDto { AppointmentId = appointmentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePrescriptionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var doctorId = User.FindFirst("sub")?.Value;
                await _prescriptionService.CreateAsync(dto, doctorId);
                return RedirectToAction("Details", "Appointments", new { id = dto.AppointmentId });
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }
    }
} 