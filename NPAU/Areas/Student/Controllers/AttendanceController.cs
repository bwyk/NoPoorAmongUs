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
        private SessionAttendance sessionAttendance;
        private DateTime date;

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
            date = DateTime.Now.Date.AddDays(1); //TODO remove hardcoding and pass in date/sessionAttendanceId dynamically
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
            List<CourseEnrollment> cEList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSessionId == targetSession.Id), includeProperties: "Student").ToList();


            List<Attendance> attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();

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
            else
            {
                List<Attendance> newAttendances = new();
                List<Attendance> matchlessAtendances = new List<Attendance>(attendance);
                // Check each enrollment for a matching attendance
                foreach (CourseEnrollment e in cEList)
                {
                    // If there are attendance records
                    if (attendance.Count > 0)
                    {
                        bool match = false;

                        foreach (Attendance a in attendance)
                        {
                            if (a.CourseEnrollment.Id == e.Id)
                            {
                                a.CourseEnrollment = e;
                                matchlessAtendances.Remove(a);
                                match = true;
                                break;
                            }
                            
                        }
                        // If no match was found make a new attendance
                        if (!match)
                        {
                            Attendance newAttendance = new Attendance()
                            {
                                CourseEnrollmentId = e.Id,
                                CourseEnrollment = e,
                                DateTaken = date,
                                SessionAttendanceId = sessionAttendance.Id
                            };
                            newAttendances.Add(newAttendance);
                        }
                    }
                    else // If there are no attendance records
                    {
                        Attendance newAttendance = new Attendance()
                        {
                            CourseEnrollmentId = e.Id,
                            CourseEnrollment = e,
                            DateTaken = date,
                            SessionAttendanceId = sessionAttendance.Id
                        };
                        newAttendances.Add(newAttendance);
                    }
                }
                if (matchlessAtendances.Count > 0)
                    _unitOfWork.Attendance.RemoveRange(matchlessAtendances);
                if (newAttendances.Count > 0)
                    _unitOfWork.Attendance.AddRange(newAttendances);
                _unitOfWork.Save();
            }
            attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();
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
                Status = status,
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
