using DoctorSystem.DTOs.Availability;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSystem.Controllers
{
    /// <summary>
    /// Controller for managing doctor availability slots.
    /// </summary>
    [Authorize(Roles = "Doctor")]
    public class AvailabilityController : Controller
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        /// <summary>
        /// Displays the list of availability slots for the current doctor.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var doctorId = User.FindFirst("DoctorId")?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return NotFound();

            var slots = await _availabilityService.GetDoctorAvailabilityAsync(doctorId);
            return View(slots);
        }

        /// <summary>
        /// Displays the form to create a new availability slot.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new availability slot.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAvailabilitySlotDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var doctorId = User.FindFirst("DoctorId")?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return NotFound();

            try
            {
                await _availabilityService.CreateAvailabilitySlotAsync(dto, doctorId);
                TempData["SuccessMessage"] = "Availability slot created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the availability slot.");
                return View(dto);
            }
        }

        /// <summary>
        /// Displays the form to edit an existing availability slot.
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var slot = await _availabilityService.GetAvailabilitySlotAsync(id);
            if (slot == null)
                return NotFound();

            var doctorId = User.FindFirst("DoctorId")?.Value;
            if (string.IsNullOrEmpty(doctorId) || slot.DoctorId != doctorId)
                return NotFound();

            var dto = new UpdateAvailabilitySlotDto
            {
                Id = slot.Id,
                DayOfWeek = slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsRecurring = slot.IsRecurring,
                SpecificDate = slot.SpecificDate,
                IsActive = slot.IsActive,
                Notes = slot.Notes
            };

            return View(dto);
        }

        /// <summary>
        /// Updates an existing availability slot.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAvailabilitySlotDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var doctorId = User.FindFirst("DoctorId")?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return NotFound();

            try
            {
                var success = await _availabilityService.UpdateAvailabilitySlotAsync(dto, doctorId);
                if (!success)
                    return NotFound();

                TempData["SuccessMessage"] = "Availability slot updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the availability slot.");
                return View(dto);
            }
        }

        /// <summary>
        /// Deletes an availability slot.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var doctorId = User.FindFirst("DoctorId")?.Value;
            if (string.IsNullOrEmpty(doctorId))
                return NotFound();

            try
            {
                var success = await _availabilityService.DeleteAvailabilitySlotAsync(id, doctorId);
                if (!success)
                    return NotFound();

                TempData["SuccessMessage"] = "Availability slot deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the availability slot.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 