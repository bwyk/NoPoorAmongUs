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

        public ViewResult Index()
        {
            return View();
        }

        public IActionResult CourseSelect()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }

        public IActionResult ViewCourseSelect()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }

        public IActionResult SessionSelect(int courseId)
        {
            IEnumerable<CourseSession> sessionList = _unitOfWork.CourseSession.GetAll(s => s.CourseId == courseId);
            return View(sessionList);
        }

        public IActionResult ViewSessionSelect(int courseId)
        {
            IEnumerable<CourseSession> sessionList = _unitOfWork.CourseSession.GetAll(s => s.CourseId == courseId);
            return View(sessionList);
        }

        public IActionResult AttendanceTable(int sessionId)
        {
            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();

            //Blank list so we can add an Attendance for each enrolled student.
            List<Attendance> attendance = new List<Attendance>();

            //Loop through the number of students that are enrolled for this SessionID.
            //For each enrolled student, create a new Attendance and update their CourseEnrollmentId so we know who they are.
            //Add it to List of Attendance objects which we can set our viewmodel with.
            for (int i = 0; i < cEList.Count(); i++)
            {
                Attendance newAttendance = new Attendance();
                newAttendance.CourseEnrollmentId = cEList[i].Id;
                newAttendance.CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(ce => ce.Id == cEList[i].Id, includeProperties: "CourseSession,Student");
                attendance.Add(newAttendance);
            }

            //May not need an CourseEnrollment list now as we have a tie in with AttendanceList holding an ID for it.
            attendanceVM = new AttendanceVM()
            {
                //CourseEnrollmentList = cEList,  //Might be able to remove.
                AttendanceList = attendance
            };

            return View(attendanceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AttendanceTable(AttendanceVM obj)
        {
            //AttendanceVM is a list of Attendance objects where each one has a CourseEnrollmentID and their checkbox data.
            //Loop through AttendanceVM and query CourseEnrollment with the id obj.CourseEnrollmentID including CourseSession and Student similar to newAttendance in the HttpGet.
            //Then do obj.CourseEnrollment = result of that query for each one which should give us a filled out Attendance object which can be added.

            if (ModelState.IsValid)
            {
                foreach(var a in obj.AttendanceList)
                {
                    a.CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(ce => ce.Id == a.CourseEnrollmentId);
                    if(a.Id == 0)
                    {
                        Attendance newAttendance = new Attendance()
                        {
                            CourseEnrollmentId = a.CourseEnrollment.Id,
                            CourseEnrollment = a.CourseEnrollment,
                            MarkedAttendance = a.MarkedAttendance,
                            Excused = a.Excused
                        };
                        _unitOfWork.Attendance.Add(newAttendance);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        _unitOfWork.Attendance.Update(a);
                        _unitOfWork.Save();
                    }
                }
                return RedirectToAction("CourseSelect");
            }
            return View(obj);
        }

        public IActionResult ViewAttendance(int sessionId)
        {
            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();

            //Blank list so we can add an Attendance for each enrolled student.
            List<Attendance> attendance = new List<Attendance>();

            //Loop through the number of students that are enrolled for this SessionID.
            //For each enrolled student, create a new Attendance and update their CourseEnrollmentId so we know who they are.
            //Add it to List of Attendance objects which we can set our viewmodel with.
            for (int i = 0; i < cEList.Count(); i++)
            {
                List<Attendance> newAttendance = _unitOfWork.Attendance.GetAll(nA => nA.CourseEnrollmentId == cEList[i].Id).ToList();
                attendance.AddRange(newAttendance);
            }

            //May not need an CourseEnrollment list now as we have a tie in with AttendanceList holding an ID for it.
            attendanceVM = new AttendanceVM()
            {
                //CourseEnrollmentList = cEList,  //Might be able to remove.
                AttendanceList = attendance
            };

            return View(attendanceVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var attendanceFromDb = _unitOfWork.Attendance.GetFirstOrDefault(a => a.Id == id);
            
            if(attendanceFromDb == null)
            {
                return NotFound();
            }

            return View(attendanceFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Attendance obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Attendance.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Attendance Updated!";
                return RedirectToAction("Index");
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
