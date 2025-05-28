using System.Threading.Tasks;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientPrescriptionsController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public PatientPrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var patientId = User.FindFirst("sub")?.Value;
            var prescriptions = await _prescriptionService.GetPrescriptionsForPatientAsync(patientId);
            return View(prescriptions);
        }
    }
} 