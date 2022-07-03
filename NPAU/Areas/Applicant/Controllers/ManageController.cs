using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Models;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class ManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public StudentVM StudentVM { get; set; } = null!;
        public ManageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {
            IEnumerable<Student> objStudentList = _unitOfWork.Student.GetAll(a => a.Status == SD.StudentStatusPending);
            return View(objStudentList);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Student.Add(obj.Student);
                _unitOfWork.Save();
                obj.Guardian.StudentId = obj.Student.Id;
                _unitOfWork.Guardian.Add(obj.Guardian);
                _unitOfWork.Save();
                TempData["success"] = "Applicant added successfully";
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

            Student student = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);
            var statusList = SD.StudentStatusList;
            foreach (SelectListItem item in statusList) 
            {
                if (item.Value == student.Status)
                    item.Selected = true;
            }
            
            StudentVM = new StudentVM()
            {
                Student = student,
                Guardian = _unitOfWork.Guardian.GetFirstOrDefault(g => g.StudentId == id),
                StudentStatusList = statusList
            };



            //if (studentFromDb == null)
            //{
            //    return NotFound();
            //}

            return View(StudentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Student.Update(obj.Student);
                _unitOfWork.Guardian.Update(obj.Guardian);
                _unitOfWork.Save();
                TempData["success"] = "Student updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Student> students;
            students = _unitOfWork.Student.GetAll();
   

            switch (status)
            {
                case "pending":
                    students = students.Where(s => s.Status == SD.StudentStatusPending);
                    break;
                case "student":
                    students = students.Where(s => s.Status == SD.StudentStatusAccepted);
                    break;
                case "rejected":
                    students = students.Where(s => s.Status == SD.StudentStatusRejected);
                    break;
                default:
                    break;
            }

            return Json(new { data = students });
        }
    }
}
