using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class GuardianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GuardianController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ViewResult Index()
        {
            IEnumerable<Guardian> objGuardianList = _unitOfWork.Guardian.GetAll();
            return View(objGuardianList);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guardian obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Guardian.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Guardian added successfully";
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

            var guardianFromDb = _unitOfWork.Guardian.GetFirstOrDefault(c => c.Id == id);

            if (guardianFromDb == null)
            {
                return NotFound();
            }

            return View(guardianFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guardian obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Guardian.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Guardian updated successfully";
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

            var guardianFromDb = _unitOfWork.Guardian.GetFirstOrDefault(c => c.Id == id);

            if (guardianFromDb == null)
            {
                return NotFound();
            }

            return View(guardianFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Guardian.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Guardian.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Guardian deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
