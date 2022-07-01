using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using System.Diagnostics;
using Utilities;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ViewResult Index()
        {
            IEnumerable<Student> objStudentList = _unitOfWork.Student.GetAll();
            return View(objStudentList);
        }

        [HttpGet]
        public ViewResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewStudent(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Student.Add(obj.Student);
                _unitOfWork.Save();
                obj.Guardian.StudentId = obj.Student.Id;
                _unitOfWork.Guardian.Add(obj.Guardian);
                _unitOfWork.Save();
                TempData["success"] = "Student added successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student obj)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.Student.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Student added successfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var studentFromDb = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);

            if (studentFromDb == null)
            {
                return NotFound();
            }

            return View(studentFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Student.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Student updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var studentFromDb = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);

            if (studentFromDb == null)
            {
                return NotFound();
            }

            return View(studentFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Student.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Student deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
