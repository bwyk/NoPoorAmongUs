using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Employee")]
    public class SocialWorkerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
