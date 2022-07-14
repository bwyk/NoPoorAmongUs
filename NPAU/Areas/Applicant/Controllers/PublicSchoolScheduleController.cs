using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using System.Diagnostics;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class PublicSchoolScheduleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public PublicSchoolScheduleVM PublicSchoolScheduleVM { get; set; } = null;
        public PublicSchoolScheduleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PublicSchoolScheduleVM obj)
        {
            if(ModelState.IsValid)
            {
                obj.PublicSchoolSchedules.Id = obj.Student.Id;
                obj.PublicSchoolSchedules.SchoolId = 1;
                _unitOfWork.PublicSchoolSchedules.Add(obj.PublicSchoolSchedules);
                _unitOfWork.Save();
                TempData["success"] = "Course Created Successfully";
                return RedirectToAction("Index");
            }
            else { var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList(); 
                Debug.WriteLine("*************************************");
                Debug.WriteLine(errors);
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Student student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);
            PublicSchoolScheduleVM = new PublicSchoolScheduleVM()
            {
                Student = student
            };
            return View(PublicSchoolScheduleVM);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var publicschoolList = _unitOfWork.PublicSchoolSchedules.GetAll();
            return Json(new { data = publicschoolList });
        }

    }
}
