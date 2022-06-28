using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class SchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
