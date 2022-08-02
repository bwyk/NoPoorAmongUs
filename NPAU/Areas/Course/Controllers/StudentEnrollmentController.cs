using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Models;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace NPAU.Controllers
{
    [Area("Course")]
    public class StudentEnrollmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentEnrollmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CourseSession> objSessionList = _unitOfWork.CourseSession.GetAll();
            return View(objSessionList);
        }
        [HttpGet]
        public IActionResult Enroll(int sessionId)
        {
            StudentEnrollmentVM studentEnrollmentVM = new StudentEnrollmentVM();
            CourseSession cs = new CourseSession();
            cs = _unitOfWork.CourseSession.GetFirstOrDefault(t => t.Id == sessionId);
            IEnumerable<Student> studentList = _unitOfWork.Student.GetAll();
            List<Student> enrolledStudents = new List<Student>();
            List<Student> unEnrolledStudents = new List<Student>();
            IEnumerable<CourseEnrollment> ce = _unitOfWork.CourseEnrollment.GetAll();
            studentEnrollmentVM.courseSession = cs;
            studentEnrollmentVM.courseEnrollment = (List<CourseEnrollment>)ce;

            foreach(CourseEnrollment course in ce)
            {
                if(course.CourseSessionId == sessionId)
                {
                    enrolledStudents.Add(course.Student);
                }
            }
            foreach(Student student in studentList)
            {
                if (!enrolledStudents.Contains(student))
                {
                    unEnrolledStudents.Add(student);
                }
            }
            studentEnrollmentVM.EnrolledStudents = enrolledStudents ;
            studentEnrollmentVM.UnEnrolledStudents = unEnrolledStudents;
            return View(studentEnrollmentVM);
        }
        public IActionResult Add(int studentId, int sessionID)
        {
            IEnumerable<CourseEnrollment> objSessionList = _unitOfWork.CourseEnrollment.GetAll();

            CourseEnrollment courseEnrollment = new CourseEnrollment();
            courseEnrollment.StudentId = studentId;
            courseEnrollment.CourseSessionId = sessionID;
            var routeValues = new RouteValueDictionary {
              { "sessionId", sessionID }
            };
            if (ModelState.IsValid)
            {
                _unitOfWork.CourseEnrollment.Add(courseEnrollment);
                _unitOfWork.Save();
                TempData["success"] = "Student enrollement added successfully";
                return RedirectToAction("Enroll", routeValues);
            }
            return RedirectToAction("Enroll", routeValues);
        }
        public IActionResult Remove(int studentId, int sessionID)
        {
            var obj = _unitOfWork.CourseEnrollment.GetFirstOrDefault(c => c.CourseSessionId == sessionID && c.StudentId == studentId);
            if (obj == null)
            {
                return NotFound();
            }
            var gradeList = _unitOfWork.Grade.GetAll().Where(x => x.CourseEnrollmentId == obj.Id);
            foreach (Grade grade in gradeList)
            {
                _unitOfWork.Grade.Remove(grade);

            }
            var cs = _unitOfWork.CourseSession.GetFirstOrDefault(c => c.Id == sessionID);
            _unitOfWork.CourseEnrollment.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Student Enrollment removed successfully.";
            var routeValues = new RouteValueDictionary {
              { "sessionId", cs.Id }
            };
            return RedirectToAction("Enroll", routeValues);
        }
    }
}
