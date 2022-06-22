using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class StudentDocsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
