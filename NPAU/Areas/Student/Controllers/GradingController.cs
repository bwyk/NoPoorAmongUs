using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Academic;
using Models.ViewModels;

namespace NPAU.Areas.Student.Controllers
{
    [Area("Student")]
    public class GradingController : Controller
    {
        

        private readonly IUnitOfWork _unitOfWork;

        public GradingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult CourseSelect()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }

        public IActionResult AssessmentSelect(int courseId)
        {
            IEnumerable<Assessment> assessmentList = _unitOfWork.Assessment.GetAll(a => a.CourseId == courseId);
            return View(assessmentList);
        }

        public IActionResult GradingTable(int assessmentId)
        {
            Assessment targetAssessment = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == assessmentId);
            GradingVM gradingVM = new GradingVM();           
            
            IEnumerable<CourseEnrollment> ceList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetAssessment.CourseId), includeProperties: "Student");
            foreach(CourseEnrollment cE in ceList)
            {
                if (!gradingVM.StudentList.Contains(cE.Student)) {
                    gradingVM.StudentList.Append(cE.Student);
                }
            }
            return View(gradingVM);
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            var gradingList = _unitOfWork.Grade.GetAll(includeProperties: "Assessment,CourseEnrollment,Course");
            return Json(new { data = gradingList });
        }
        #endregion
    }


}
