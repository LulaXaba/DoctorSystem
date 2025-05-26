using Microsoft.AspNetCore.Mvc;

namespace DoctorSytem.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
