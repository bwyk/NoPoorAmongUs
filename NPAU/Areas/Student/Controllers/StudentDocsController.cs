using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentDocsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
