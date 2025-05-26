using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoctorSystem.Data;
using DoctorSystem.Helpers;
using DoctorSystem.Models;
using DoctorSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _imagePath;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _imagePath = Path.Combine(env.WebRootPath, "images");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

            var doctorList = doctors.Select(d => new DoctorDisplay
            {
                Id = d.Id,
                UserName = d.UserName,
                ImageFileName = System.IO.File.Exists(Path.Combine(_imagePath, $"{d.UserName.ToLower()}.jpg"))
                    ? $"{d.UserName.ToLower()}.jpg"
                    : "default-doctor.png"
            }).ToList();

            var model = new AppointmentViewModel
            {
                StartDate = DateTime.Today,
                ProviderList = doctorList,
                AvailableTimeSlots = TimeSlotHelper.GenerateSlots(
                    DateTime.Today,
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(17, 0, 0))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (!ModelState.IsValid || model.SelectedDoctors == null || !model.SelectedDoctors.Any())
            {
                var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

                model.ProviderList = doctors.Select(d => new DoctorDisplay
                {
                    Id = d.Id,
                    UserName = d.UserName,
                    ImageFileName = System.IO.File.Exists(Path.Combine(_imagePath, $"{d.UserName.ToLower()}.jpg"))
                        ? $"{d.UserName.ToLower()}.jpg"
                        : "default-doctor.png"
                }).ToList();

                model.AvailableTimeSlots = TimeSlotHelper.GenerateSlots(
                    model.StartDate,
                    new TimeSpan(9, 0, 0),
                    new TimeSpan(17, 0, 0));

                if (model.SelectedDoctors == null || !model.SelectedDoctors.Any())
                {
                    ModelState.AddModelError("", "You must select at least one doctor.");
                }

                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            foreach (var doctorId in model.SelectedDoctors)
            {
                var appointment = new Appointment
                {
                    ProviderId = doctorId,
                    CustomerId = user.Id,
                    Department = model.Department!,
                    StartTime = model.StartDate.Date + model.StartTime,
                    EndTime = model.StartDate.Date + model.EndTime,
                    Description = model.Description,
                    Status = "Pending"
                };

                _context.Appointments.Add(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyAppointments));
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointments()
        {
            var user = await _userManager.GetUserAsync(User);

            var appointments = await _context.Appointments
                .Where(a => a.CustomerId == user.Id)
                .Include(a => a.Provider) // Include doctor details
                .ToListAsync();

            return View(appointments);
        }
    }
}
