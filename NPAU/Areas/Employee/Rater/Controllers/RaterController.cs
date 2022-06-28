using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Employee")]
    public class RaterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
