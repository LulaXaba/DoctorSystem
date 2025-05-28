using System.Threading.Tasks;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class MedicalRecordsController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        public async Task<IActionResult> Index()
        {
            var patientId = User.FindFirst("sub")?.Value;
            var medicalRecord = await _medicalRecordService.GetPatientMedicalRecordAsync(patientId);
            return View(medicalRecord);
        }
    }
} 