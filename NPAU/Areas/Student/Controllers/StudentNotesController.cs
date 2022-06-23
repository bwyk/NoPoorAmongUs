using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentNotesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
