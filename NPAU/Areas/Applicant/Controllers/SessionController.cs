﻿using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Academic;
using Models.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using Utilities;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class SessionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult GetAllSessions(string status)
        {
            string userId = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            string[] args = new string[0];
            string role = "";
            string filter1;
            if (status != null)
            {
                args = status.Split('_');
                role = args[0];
            }

            if (User.IsInRole(SD.Role_Instructor) || User.IsInRole(SD.Role_Admin))
            {
                IEnumerable<CourseSession> courseSessions;
                IEnumerable<Course> course;

                filter1 = args[1];
                switch (filter1)
                {
                    case "your":
                        course = _unitOfWork.Course.GetAll(c => c.InstructorId == userId);
                        courseSessions = _unitOfWork.CourseSession.GetAll(s => course.Select(c => c.Id).Contains(s.CourseId));
                        break;
                    case "public":
                        var publicSchools = _unitOfWork.School.GetAll(s => s.Name != SD.SchoolBoanne);
                        course = _unitOfWork.Course.GetAll(c => publicSchools.Select(s => s.Id).Contains(c.Id));
                        courseSessions = _unitOfWork.CourseSession.GetAll(s => course.Select(c => c.Id).Contains(s.CourseId));
                        break;
                    case "private":
                        var boanneSchool = _unitOfWork.School.GetFirstOrDefault(s => s.Name == SD.SchoolBoanne);
                        course = _unitOfWork.Course.GetAll(c => c.SchoolId == boanneSchool.Id);
                        courseSessions = _unitOfWork.CourseSession.GetAll(s => course.Select(c => c.Id).Contains(s.CourseId));
                        break;
                    default://case "all":
                        courseSessions = _unitOfWork.CourseSession.GetAll();
                        break;
                }
                return Json(new { data = courseSessions });
            }
            return Json(new { data = "" });
        }


        public IActionResult Upsert(int? id)
        {
            SessionVM pubScheduleVM = new();


            if (id == null || id == 0)
            {
                pubScheduleVM.CourseSession = new();
                return View(pubScheduleVM);
            }
            else
            {
                //studentVM.Student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);
                pubScheduleVM.CourseSession = _unitOfWork.CourseSession.GetFirstOrDefault(c => c.Id == id);
                return View(pubScheduleVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SessionVM obj)
        {
            if (ModelState.IsValid)
            {
                bool match;
                var enteredStudents = new List<Student>();
                if (!string.IsNullOrEmpty(obj.CourseEnrollmentStudentsJSON))
                {
                    enteredStudents = JsonConvert.DeserializeObject<List<Student>>(obj.CourseEnrollmentStudentsJSON);
                }
                // If new CourseSession
                if (obj.CourseSession.Id == 0)
                {
                    _unitOfWork.CourseSession.Add(obj.CourseSession);
                    _unitOfWork.Save(); // breaks here
                }
                else
                {
                    _unitOfWork.CourseSession.Update(obj.CourseSession);
                    _unitOfWork.Save();
                }

                CourseSession courseSession = _unitOfWork.CourseSession.GetFirstOrDefault(session => (session.CourseName == obj.CourseSession.CourseName) &&
                                                                (session.StartTime == obj.CourseSession.StartTime));

                // All enrollments involving the session
                var enrollments = _unitOfWork.CourseEnrollment.GetAll(r => r.CourseSessionId == courseSession.Id);
                // All students that are included in the enrollments
                var students = _unitOfWork.Student.GetAll(s => enrollments.Select(enrollments => enrollments.StudentId).Contains(s.Id));

                List<Student> newStudentEnrollments = new();
                // Check for existing or new relationships
                foreach (Student s in enteredStudents ?? Enumerable.Empty<object>()) // If null do nothing
                {
                    match = false;
                    //// If this is a new guardian
                    //if (s.Id == -1)
                    //{
                    //    _unitOfWork.Guardian.Add(new Guardian()
                    //    {
                    //        FirstName = g.FirstName,
                    //        LastName = g.LastName,
                    //        Relationship = g.Relationship
                    //    });
                    //    _unitOfWork.Save();
                    //}

                    // check for any existing student enrollment
                    foreach (var enrollment in enrollments)
                        if (enrollment.StudentId == s.Id)
                        {
                            match = true;
                            break;
                        }

                    // If there are none make a new enrollment
                    if (!match)
                    {
                        Student student = _unitOfWork.Student.GetFirstOrDefault(student => student.Id == s.Id);
                        _unitOfWork.CourseEnrollment.Add(new CourseEnrollment()
                        {
                            Student = student,
                            StudentId = student.Id,
                            CourseSession = courseSession,
                            CourseSessionId = courseSession.Id,
                        });
                        _unitOfWork.Save();
                        newStudentEnrollments.Add(student);
                        enrollments = _unitOfWork.CourseEnrollment.GetAll(c => c.CourseSessionId == obj.CourseSession.Id);
                    }
                }


                // Check if any enrollments were removed
                foreach (CourseEnrollment e in enrollments)
                    // If there are not any entered enrollments that match a previous
                    if (!enteredStudents.Any(s => s.Id == e.StudentId))
                    {
                        if (!newStudentEnrollments.Any(s => s.Id == e.StudentId))
                            _unitOfWork.CourseEnrollment.Remove(e);
                    }


                TempData["success"] = "Course enrollments updated successfully!";


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            obj.PotentialStudents = _unitOfWork.Student.GetAll();
            // TODO filter >.<


            return View(obj);
        }

        [HttpGet]
        public IActionResult GetPotentialEnrollments(int courseSessionId)
        {
            // All enrollments involving the session
            var enrollments = _unitOfWork.CourseEnrollment.GetAll(r => r.CourseSessionId == courseSessionId);
            // All students that are NOT included in the enrollments
            var students = _unitOfWork.Student.GetAll(s => !enrollments.Select(enrollments => enrollments.StudentId).Contains(s.Id));

            return Json(new { data = students });
        }
        [HttpGet]
        public IActionResult GetCurrentEnrollments(int courseSessionId)
        {
            // All enrollments involving the session
            var enrollments = _unitOfWork.CourseEnrollment.GetAll(r => r.CourseSessionId == courseSessionId);
            // All students that are included in the enrollments
            var students = _unitOfWork.Student.GetAll(s => enrollments.Select(enrollments => enrollments.StudentId).Contains(s.Id));

            return Json(new { data = students });
        }
    }
}

