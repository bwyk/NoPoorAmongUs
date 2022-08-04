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

        public IActionResult MarkAttendance(int sessionId, int? attendanceId, string? status)
        {
            // DateTime.Now.Date to standardize the time so we can match off of the date.
            DateTime date = DateTime.Now.Date.AddDays(1); //TODO remove hardcoding and pass in date/sessionAttendanceId dynamically
            SessionAttendance sessionAttendance = null;
            bool fromEdit;
            // If we are not editing an already existing record
            if (attendanceId == null)
            {
                fromEdit = false; // Flag for returning the user after post 
                // SessionAttendance is just a way to group attendances so you can pull all attendances for a given session and date
                sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.CourseSessionId == sessionId && sa.DateTaken == date, includeProperties: "CourseSession");
            }
            else {
                fromEdit = true; // Flag for returning the user after post 
                sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.Id == attendanceId, includeProperties: "CourseSession");
                sessionId = sessionAttendance.CourseSessionId;
            }

            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();


            List<Attendance> attendance = new();

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
                attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();
            }
            else
            {
                attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();
                foreach(Attendance a in attendance)
                {
                    for (int i = 0; i < cEList.Count; i++)
                    {
                        if (a.CourseEnrollment.Id == cEList[i].Id)
                        {
                            a.CourseEnrollment = cEList[i];
                            cEList.RemoveAt(i);
                            break;
                        }
                    }
                }
                
            }

            attendanceVM = new AttendanceVM()
            {
                Status = status,
                isFromEdit = fromEdit,
                SessionAttendance = sessionAttendance,
                AttendanceList = attendance
            };

            return View(attendanceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAttendance(AttendanceVM obj)
        {

            if (ModelState.IsValid)
            {
                foreach(var a in obj.AttendanceList)
                {
                    // Only update as they were all made in the get
                    _unitOfWork.Attendance.Update(a);
                    _unitOfWork.Save();
                }
                if (obj.isFromEdit)
                    return RedirectToAction("ViewAttendance", new { sessionId = obj.SessionAttendance.CourseSessionId, status = obj.Status });
                else
                    return RedirectToAction("Index", "Session", new { area = "Applicant", status = obj.Status});
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult ViewAttendance(int sessionId, string? status)
        {
            AllAttendanceVM allAttendanceVM = new()
            {
                //AllSessionAttendance = sessionAttendances,
                AllSessionAttendance = new(),

            };
            List<SessionAttendance> sessionAttendances = _unitOfWork.SessionAttendance.GetAll(sa => sa.CourseSessionId == sessionId).ToList();
            CourseSession targetSession = _unitOfWork.CourseSession.GetFirstOrDefault(cS => cS.Id == sessionId);

            foreach (SessionAttendance sa in sessionAttendances)
            {
                AttendanceVM attendanceVM = new AttendanceVM();
                attendanceVM.Status = status;
                attendanceVM.SessionAttendance = sa;
                attendanceVM.AttendanceList = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sa.Id).ToList();
                List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetSession.CourseId), includeProperties: "Student").ToList();

                foreach (var attendance in attendanceVM.AttendanceList)
                {
                    for(int i = 0; i < cEList.Count; i++)
                    {
                        if (attendance.CourseEnrollment.Id == cEList[i].Id)
                        {
                            attendance.CourseEnrollment = cEList[i];
                            cEList.RemoveAt(i);
                            break;
                        }
                    }
                    
                };
                allAttendanceVM.AllSessionAttendance.Add(attendanceVM);
            }

            return View(allAttendanceVM);
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

        private Attendance GetStudent(int id)
        {
            var attendance = _unitOfWork.Attendance.GetFirstOrDefault(a => a.Id == id);
            return attendance;
        }
        #endregion

    }
}
