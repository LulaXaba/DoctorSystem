using Microsoft.AspNetCore.Mvc;

namespace DoctorSytem.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
