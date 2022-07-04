using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels;
namespace NPAU.Controllers
{
    [Area("Student")]
    public class AssessmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            AssessmentVM assessmentVM = new()
            {
                Assessment = new(),
                CourseList = _unitOfWork.Course.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };

            if (id == null || id == 0)
            {
                return View(assessmentVM);
            }
            else
            {
                assessmentVM.Assessment = _unitOfWork.Assessment.GetFirstOrDefault(c => c.Id == id);
                return View(assessmentVM);
            }

            return View(assessmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AssessmentVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Assessment.Id == 0)
                {
                    _unitOfWork.Assessment.Add(obj.Assessment);
                    TempData["success"] = "Assessment created successfully!";

                }
                else
                {
                    _unitOfWork.Assessment.Update(obj.Assessment);
                    TempData["success"] = "Assessment updated successfully!";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            obj.CourseList = _unitOfWork.Course.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            

            return View(obj);
        }

        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            var assessmentList = _unitOfWork.Assessment.GetAll(includeProperties: "Course");
            return Json(new { data = assessmentList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Assessment.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Assessment deleted successfully!" });

        }

        #endregion
    }
}
