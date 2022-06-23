using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
