using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
