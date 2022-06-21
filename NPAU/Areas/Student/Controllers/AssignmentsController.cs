using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class AssignmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
