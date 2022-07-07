using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;

namespace NPAU.Areas.Student.Controllers
{
    [Area("Student")]
    public class GradeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            GradeVM gradeVM = new()
            {
                Grade = new(),
                AssessmentList = _unitOfWork.Assessment.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CourseEnrollmentList = _unitOfWork.CourseEnrollment.GetAll().Select(j => new SelectListItem
                {
                    Text = j.StudentId.ToString(),
                    Value = j.Id.ToString()
                })

            };

            if (id == null || id == 0)
            {
                return View(gradeVM);
            }
            else
            {
                gradeVM.Grade= _unitOfWork.Grade.GetFirstOrDefault(g => g.Id == id);
                return View(gradeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(GradeVM obj)
        {
            obj.Grade.Assessment = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == obj.Grade.AssessmentId, includeProperties: "Course");
            obj.Grade.CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(a => a.Id == obj.Grade.CourseEnrollmentId, includeProperties: "Student,CourseSession");
            ModelState.Clear();
            TryValidateModel(obj);
            
            if (ModelState.IsValid)
            {
                if (obj.Grade.Id == 0)
                {
                    _unitOfWork.Grade.Add(obj.Grade);
                    TempData["success"] = "Assessment graded successfully!";

                }
                else
                {
                    _unitOfWork.Grade.Update(obj.Grade);
                    TempData["success"] = "Grade updated successfully!";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            obj.AssessmentList = _unitOfWork.Assessment.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            obj.CourseEnrollmentList = _unitOfWork.CourseEnrollment.GetAll().Select(j => new SelectListItem
            {
                Text = j.StudentId.ToString(),
                Value = j.Id.ToString()
            });

            return View(obj);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var gradeList = _unitOfWork.Grade.GetAll(includeProperties: "Assessment,CourseEnrollment");
            return Json(new { data = gradeList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Grade.GetFirstOrDefault(g => g.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Grade.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Grade deleted successfully!" });
        }
        #endregion
    }
}
