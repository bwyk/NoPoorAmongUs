using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Employee")]
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
