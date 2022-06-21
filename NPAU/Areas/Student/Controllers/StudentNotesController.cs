using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class StudentNotesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
