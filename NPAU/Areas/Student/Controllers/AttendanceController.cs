using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Academic;
using Models.ViewModels;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class AttendanceController : Controller
    {
        [BindProperty]
        private AttendanceVM attendanceVM { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        public AttendanceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult CourseSelect()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }

        public IActionResult SessionSelect(int courseId)
        {
            IEnumerable<CourseSession> sessionList = _unitOfWork.CourseSession.GetAll(s => s.CourseId == courseId);
            return View(sessionList);
        }

        public IActionResult AttendanceTable(int sessionId)
        {
            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();

            attendanceVM = new AttendanceVM();
            attendanceVM.CourseEnrollmentList = cEList;

            //foreach (CourseEnrollment cE in cEList)
            //{
            //    Models.Student student = cE.Student;
            //}

            return View(attendanceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AttendanceTable(AttendanceVM obj)
        {
            if (ModelState.IsValid)
            {
                if(obj.Attendance.Id == 0)
                {
                    _unitOfWork.Attendance.Add(obj.Attendance);
                }
                else
                {
                    _unitOfWork.Attendance.Update(obj.Attendance);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Attendance has been recorded.";
                return RedirectToAction("CourseSelect");
            }
            return View(obj);
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            var attendanceList = _unitOfWork.Attendance.GetAll();
            return Json(new {data = attendanceList});
        }
        #endregion

    }
}
