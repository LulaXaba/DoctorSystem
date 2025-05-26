using Microsoft.AspNetCore.Mvc;

namespace DoctorSytem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
