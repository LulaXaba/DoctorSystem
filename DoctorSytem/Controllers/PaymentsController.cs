using DoctorSystem.DTOs;
using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Create()
        {
            var patientId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var appointments = _paymentService.GetPatientAppointments(patientId);
            ViewBag.Appointments = appointments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                var patientId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                var appointments = _paymentService.GetPatientAppointments(patientId);
                ViewBag.Appointments = appointments;
                return View(dto);
            }

            var patientIdPost = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var payment = await _paymentService.CreatePaymentAsync(dto, patientIdPost);

            return RedirectToAction(nameof(MyPayments));
        }

        public async Task<IActionResult> MyPayments()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var payments = await _paymentService.GetPatientPaymentsAsync(patientId);
            return View(payments);
        }
    }
} 