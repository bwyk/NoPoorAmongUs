using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Academic;
using Models.ViewModels;
using System.Diagnostics;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    //[Route("api/[controller]")]
    //[ApiController]
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
        public IActionResult Schedule()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PublicSchoolScheduleVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.PublicSchoolSchedules.StudentId = obj.Student.Id;
                _unitOfWork.PublicSchoolSchedules.Add(obj.PublicSchoolSchedules);
                _unitOfWork.Save();
                TempData["success"] = "Public School Session Created Successfully";
                //return RedirectToAction("Schedule", new {id = obj.Student.Id});
                return RedirectToAction("Index", new { id = obj.Student.Id });
            }

            return View(obj);
        }

        //[HttpGet]
        public IActionResult Index(int? id)
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
        public IActionResult GetAll(int id)
        {
            //id = 2;
            //IEnumerable<PublicSchoolSchedule> publicSchoolSchedules;
            var publicSchoolSchedules = _unitOfWork.PublicSchoolSchedules.GetAll().Where(u => u.StudentId == id);
            //publicSchoolSchedules = publicSchoolSchedules.Where(u => u.StudentId == id);
            return Json(new { data = publicSchoolSchedules });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PublicSchoolScheduleVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.PublicSchoolSchedules.StudentId = obj.Student.Id;
                _unitOfWork.PublicSchoolSchedules.Update(obj.PublicSchoolSchedules);
                _unitOfWork.Save();
                TempData["success"] = "Public School Session Updated Successfully";
                return RedirectToAction("Schedule", new { id = obj.Student.Id });
            }
            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            IEnumerable<PublicSchoolSchedule> pl = _unitOfWork.PublicSchoolSchedules.GetAll();
            Student student = new();
            foreach (var item in pl)
            {
                if (item.Id == id)
                {
                    student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == item.StudentId);
                }
            }

            //Student student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);
            PublicSchoolScheduleVM = new PublicSchoolScheduleVM()
            {
                PublicSchoolSchedules = _unitOfWork.PublicSchoolSchedules.GetFirstOrDefault(p => p.Id == id),
                Student = student
            };

            return View(PublicSchoolScheduleVM);

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.PublicSchoolSchedules.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.PublicSchoolSchedules.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Public School Session Deleted Successfully" });
        }

    }
}
