﻿using DataAccess.Repository.IRepository;
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
            studentEnrollmentVM.courseSession = cs;
            studentEnrollmentVM.Student = (List<Student>)studentList;
 
           
            return View(studentEnrollmentVM);
        }
        public IActionResult Add(int studentId, int sessionID)
        {
            CourseEnrollment courseEnrollment = new CourseEnrollment();
            if (ModelState.IsValid)
            {
                _unitOfWork.CourseEnrollment.Add(courseEnrollment);
                _unitOfWork.Save();
                TempData["success"] = "Student enrollement added successfully";
                return RedirectToAction("Enroll");
            }
            return RedirectToAction("Enroll");
        }

        
    }
}
