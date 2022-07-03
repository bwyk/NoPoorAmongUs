using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace NPAU.Controllers
{
    [Area("Employee")]
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Instructor> objInstructorList = _unitOfWork.Instructor.GetAll();
            return View(objInstructorList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Instructor obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Instructor.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Instructor added successfully!";
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

            var instructorFromDb = _unitOfWork.Instructor.GetFirstOrDefault(t => t.Id == id);

            if (instructorFromDb == null)
            {
                return NotFound();
            }

            return View(instructorFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Instructor obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Instructor.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Instructor updated successfully!";
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

            var instructorFromDb = _unitOfWork.Instructor.GetFirstOrDefault(t => t.Id == id);

            if (instructorFromDb == null)
            {
                return NotFound();
            }

            return View(instructorFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Instructor.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Instructor.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Instructor deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
