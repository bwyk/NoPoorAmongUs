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
            date = DateTime.Now.Date;

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
                //_unitOfWork.SessionAttendance.Add(sessionAttendance);
                //_unitOfWork.Save();
                //sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.CourseSessionId == sessionId && sa.DateTaken == date, includeProperties: "CourseSession");

                
                for (int i = 0; i < cEList.Count(); i++)
                {
                    Attendance newAttendance = new Attendance()
                    {
                        CourseEnrollmentId = cEList[i].Id,
                        CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(ce => ce.Id == cEList[i].Id, includeProperties: "CourseSession,Student"),
                        DateTaken = date,
                    };

                    attendance.Add(newAttendance);
                }
                //_unitOfWork.Attendance.AddRange(attendance);
                //_unitOfWork.Save();
            }
            else
            {
                attendance = RemoveDuplicateRecords(sessionAttendance.Id);

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
        private List<Attendance> RemoveDuplicateRecords(int sessionId)
        {
            List<Attendance> attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionId).ToList();
            List<Attendance> singleAttendances = attendance.GroupBy(x => x.CourseEnrollmentId).Select(x => x.First()).ToList();
            List<Attendance> dupAttendance = attendance.Where(x => !singleAttendances.Select(sa => sa.Id).Contains(x.Id)).ToList();
            _unitOfWork.Attendance.RemoveRange(dupAttendance);
            _unitOfWork.Save();
            return _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionId).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAttendance(AttendanceVM obj)
        {

            if (ModelState.IsValid)
            {
                // Check to see if there is an already record for the date and session
                SessionAttendance sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.DateTaken == obj.SessionAttendance.DateTaken
                        && sa.CourseSessionId == obj.SessionAttendance.CourseSessionId);
                if (sessionAttendance == null)
                {
                    // If no record exists make a new one
                    SessionAttendance newAttendance = new SessionAttendance()
                    {
                        DateTaken = obj.SessionAttendance.DateTaken,
                        CourseSessionId = obj.SessionAttendance.CourseSessionId,
                        CourseSession = obj.SessionAttendance.CourseSession,
                    };
                    _unitOfWork.SessionAttendance.Add(newAttendance);
                    _unitOfWork.Save();
                    sessionAttendance = _unitOfWork.SessionAttendance.GetFirstOrDefault(sa => sa.DateTaken == obj.SessionAttendance.DateTaken 
                        && sa.CourseSessionId ==  obj.SessionAttendance.CourseSessionId);
                    // var dupes = RemoveDuplicateRecords(sessionAttendance.Id);
                    foreach (var a in obj.AttendanceList)
                    {
                        // Only update as they were all made in the get
                        a.DateTaken = sessionAttendance.DateTaken;
                        a.SessionAttendanceId = sessionAttendance.Id;
                        _unitOfWork.Attendance.Update(a);
                        _unitOfWork.Save();
                    }

                }
                else
                {
                    List<Attendance> attendance = _unitOfWork.Attendance.GetAll(a => a.SessionAttendanceId == sessionAttendance.Id).ToList();
                    if (attendance[0].Id != obj.AttendanceList[0].Id) // If there were already attendance records, remove the new ones.
                        _unitOfWork.Attendance.RemoveRange(obj.AttendanceList);

                    RecursiveMatch(attendance, obj.AttendanceList); // Match old and new records and write them to the db
                        
                    _unitOfWork.Save();                                  
                }
                
                
                if (obj.isFromEdit)
                    return RedirectToAction("ViewAttendance", new { sessionId = obj.SessionAttendance.CourseSessionId, status = obj.Status });
                else
                    return RedirectToAction("Index", "Session", new { area = "Applicant", status = obj.Status});
            }
            return View(obj);
        }

        /// <summary>
        /// Created to update already existing Attendance records instead of adding new ones.
        /// <para>Matches values based on CourseEnrollmentId, updates objects of the first list with values of the second and writes it to the db.</para>
        /// </summary>
        /// <param name="attendanceA"></param>
        /// <param name="attendanceB"></param>
        private void RecursiveMatch(List<Attendance> attendanceA, List<Attendance> attendanceB)
        {
            bool match = false;
            foreach (var a in attendanceA)
            {
                foreach (var b in attendanceB)
                {
                    if (a.CourseEnrollmentId == b.CourseEnrollmentId)
                    {
                        a.DateTaken = b.DateTaken;
                        a.Excused = b.Excused;
                        a.MarkedAttendance = b.MarkedAttendance;
                        _unitOfWork.Attendance.Update(a);
                        attendanceA.Remove(a);
                        attendanceB.Remove(b);
                        match = true;
                        break;
                    }
                    
                }
                if (match)
                {
                    if(attendanceA.Count() > 0 && attendanceB.Count() > 0)
                        RecursiveMatch(attendanceA, attendanceB);
                    break;

                }
            }
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
                if(attendanceVM.AttendanceList.Count > 0)
                    allAttendanceVM.AllSessionAttendance.Add(attendanceVM);
                else { 
                    _unitOfWork.SessionAttendance.Remove(sa);
                }
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
