using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class TermController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TermController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Term> objTermList = _unitOfWork.Term.GetAll();
            return View(objTermList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Term obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Term.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Term added successfully!";
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

            var termFromDb = _unitOfWork.Term.GetFirstOrDefault(t => t.Id == id);

            if (termFromDb == null)
            {
                return NotFound();
            }

            return View(termFromDb);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Term obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Term.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Term updated successfully!";
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

            var termFromDb = _unitOfWork.Term.GetFirstOrDefault(t => t.Id == id);

            if (termFromDb == null)
            {
                return NotFound();
            }

            return View(termFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Term.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Term.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Term deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
