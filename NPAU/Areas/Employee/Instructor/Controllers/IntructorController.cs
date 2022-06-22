using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Employee")]
    public class IntructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
