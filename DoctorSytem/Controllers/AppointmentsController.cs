using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DoctorSystem.Models.ViewModels;
using DoctorSystem.DTOs.Appointments;
using DoctorSystem.Services.Interfaces;
using DoctorSystem.Validators;
using FluentValidation;
using System.Security.Claims;

namespace DoctorSystem.Controllers
{
    /// <summary>
    /// Controller for handling appointment-related operations.
    /// Requires authentication for all actions.
    /// </summary>
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IValidator<CreateAppointmentDto> _validator;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IUserService userService,
            IValidator<CreateAppointmentDto> validator)
        {
            _appointmentService = appointmentService;
            _userService = userService;
            _validator = validator;
        }

        /// <summary>
        /// Displays the appointment creation form.
        /// </summary>
        /// <returns>The appointment creation view with populated doctor list and time slots</returns>
        public async Task<IActionResult> Create()
        {
            var viewModel = new AppointmentViewModel
            {
                ProviderList = await _userService.GetDoctorsAsync(),
                AvailableTimeSlots = GetDefaultTimeSlots()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the appointment creation form submission.
        /// </summary>
        /// <param name="viewModel">The appointment data from the form</param>
        /// <returns>Redirects to appointment list on success, returns to form on failure</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ProviderList = await _userService.GetDoctorsAsync();
                viewModel.AvailableTimeSlots = GetDefaultTimeSlots();
                return View(viewModel);
            }

            var dto = new CreateAppointmentDto
            {
                DoctorIds = viewModel.SelectedDoctors,
                Department = viewModel.Department,
                AppointmentDate = viewModel.StartDate,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                Notes = viewModel.Description
            };

            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                viewModel.ProviderList = await _userService.GetDoctorsAsync();
                viewModel.AvailableTimeSlots = GetDefaultTimeSlots();
                return View(viewModel);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _appointmentService.CreateAppointmentAsync(dto, userId);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the list of appointments for the current user.
        /// </summary>
        /// <returns>The appointment list view</returns>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var appointments = await _appointmentService.GetPatientAppointmentsAsync(userId);
            return View(appointments);
        }

        /// <summary>
        /// Generates default time slots for appointment scheduling (09:00-17:00).
        /// </summary>
        /// <returns>List of available time slots</returns>
        private List<TimeSpan> GetDefaultTimeSlots()
        {
            var slots = new List<TimeSpan>();
            for (int hour = 9; hour < 17; hour++)
            {
                slots.Add(new TimeSpan(hour, 0, 0));
            }
            return slots;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.CancelAppointmentAsync(id, userId);
            
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 