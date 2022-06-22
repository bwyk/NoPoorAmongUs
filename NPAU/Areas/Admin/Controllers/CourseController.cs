using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
