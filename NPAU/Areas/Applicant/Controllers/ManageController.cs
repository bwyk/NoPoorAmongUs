using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
