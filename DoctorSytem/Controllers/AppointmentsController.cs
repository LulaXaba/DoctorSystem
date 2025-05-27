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
        /// Displays the list of appointments for the current user.
        /// </summary>
        /// <returns>The appointment list view</returns>
        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var appointments = await _appointmentService.GetPatientAppointmentsAsync(userId);
            return View("Index", appointments);
        }

        /// <summary>
        /// Displays the appointment cancellation form.
        /// </summary>
        /// <param name="id">ID of the appointment to cancel</param>
        /// <returns>The cancellation form view</returns>
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || (appointment.PatientId != userId && appointment.DoctorId != userId))
            {
                return Unauthorized();
            }

            var viewModel = new CancelAppointmentViewModel
            {
                AppointmentId = id,
                Appointment = appointment
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles the appointment cancellation form submission.
        /// </summary>
        /// <param name="viewModel">The cancellation data from the form</param>
        /// <returns>Redirects to appointment list on success, returns to form on failure</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(CancelAppointmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Appointment = await _appointmentService.GetAppointmentByIdAsync(viewModel.AppointmentId);
                return View(viewModel);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var dto = new CancelAppointmentDto
            {
                AppointmentId = viewModel.AppointmentId,
                Reason = viewModel.Reason
            };

            var result = await _appointmentService.CancelAppointmentAsync(dto, userId);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
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

        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckInDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _appointmentService.CheckInAsync(dto, userId);

            if (!result)
            {
                TempData["Error"] = "Check-in failed. Please ensure the appointment is scheduled for today and hasn't been checked in already.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "You have checked in successfully.";
            return RedirectToAction("Index");
        }
    }
} 