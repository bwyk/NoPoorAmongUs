using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Academic;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class SchoolController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SchoolController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<School> objSchoolList = _unitOfWork.School.GetAll();
            return View(objSchoolList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(School obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.School.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "School added successfully!";
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

            var schoolFromDb = _unitOfWork.School.GetFirstOrDefault(t => t.Id == id);

            if (schoolFromDb == null)
            {
                return NotFound();
            }

            return View(schoolFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(School obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.School.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "School updated successfully!";
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

            var schoolFromDb = _unitOfWork.School.GetFirstOrDefault(t => t.Id == id);

            if (schoolFromDb == null)
            {
                return NotFound();
            }

            return View(schoolFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.School.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.School.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "School deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
