using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace NPAU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Subject> objSubjectList = _unitOfWork.Subject.GetAll();
            return View(objSubjectList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subject obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Subject.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Subject added successfully!";
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

            var subjectFromDb = _unitOfWork.Subject.GetFirstOrDefault(t => t.Id == id);

            if (subjectFromDb == null)
            {
                return NotFound();
            }

            return View(subjectFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Subject obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Subject.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Subject updated successfully!";
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

            var subjectFromDb = _unitOfWork.Subject.GetFirstOrDefault(t => t.Id == id);

            if (subjectFromDb == null)
            {
                return NotFound();
            }

            return View(subjectFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Subject.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Subject.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Subject deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
