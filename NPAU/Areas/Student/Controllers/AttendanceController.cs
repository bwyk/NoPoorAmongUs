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
            CourseSessionSelectVM courseSessionSelectVM = new()
            {
                CourseId = courseId,
                SessionList = _unitOfWork.CourseSession.GetAll(s => s.CourseId == courseId).ToList()
            };
            return View(courseSessionSelectVM);
        }

        public IActionResult ViewSessionSelect(int courseId)
        {
            IEnumerable<CourseSession> sessionList = _unitOfWork.CourseSession.GetAll(s => s.CourseId == courseId);
            return View(sessionList);
        }

        public IActionResult AttendanceTable(int sessionId)
        {
            // DateTime.Now.Date to standardize the time so we can match off of the date.
            DateTime date = DateTime.Now.Date; //TODO remove hardcoding and pass in date/sessionAttendanceId dynamically

            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();
            
            // SessionAttendance is just a way to group attendances so you can pull all attendances for a given session and date
            SessionAttendance sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.CourseSessionId == sessionId && sa.DateTaken == date, includeProperties: "CourseSession");
            List<Attendance> attendance;

            // If attendance has not been taken for the given session on the given date
            if (sessionAttendance == null)
            {
                sessionAttendance = new SessionAttendance() 
                {
                    CourseSessionId = sessionId,
                    CourseSession = targetSession,
                    DateTaken = date, // note needs to be "DateTime.Now.Date;" or you will not be able to match it from the db E.g. '{8/1/2022 12:00:00 AM} != {8/1/2022 12:00:01 AM}'
                };
                _unitOfWork.SessionAttendance.Add(sessionAttendance);
                _unitOfWork.Save();
                sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.CourseSessionId == sessionId && sa.DateTaken == date, includeProperties: "CourseSession");
 
                //Blank list so we can add an Attendance for each enrolled student.
                attendance = new();
                //Loop through the number of students that are enrolled for this SessionID.
                //For each enrolled student, create a new Attendance and update their CourseEnrollmentId so we know who they are.
                //Add it to List of Attendance objects which we can set our viewmodel with.
                for (int i = 0; i < cEList.Count(); i++)
                {
                    Attendance newAttendance = new Attendance()
                    {
                        CourseEnrollmentId = cEList[i].Id,
                        CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(ce => ce.Id == cEList[i].Id, includeProperties: "CourseSession,Student"),
                        DateTaken = date,
                        SessionAttendanceId = sessionAttendance.Id
                    };
                    
                    attendance.Add(newAttendance);
                }
                _unitOfWork.Attendance.AddRange(attendance);
                _unitOfWork.Save();
            }
         
            attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();

            attendanceVM = new AttendanceVM()
            {
                SessionAttendance = sessionAttendance,
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
                    // Only update as they were all made in the get
                    _unitOfWork.Attendance.Update(a);
                    _unitOfWork.Save();
                }
                return RedirectToAction("CourseSelect");
            }
            return View(obj);
        }

        [HttpGet]
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ViewAttendance(AttendanceVM obj)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }
        //    return View(obj);
        //}

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

        private Attendance GetStudent(int id)
        {
            var attendance = _unitOfWork.Attendance.GetFirstOrDefault(a => a.Id == id);
            return attendance;
        }
        #endregion

    }
}
