using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;

namespace NPAU.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class PublicSchoolScheduleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublicSchoolScheduleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var publicschoolList = _unitOfWork.PublicSchoolSchedules.GetAll();
            return Json(new { data = publicschoolList });
        }

    }
}
