using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.Academic;

namespace NPAU.Areas.Admin.Controllers
{
    [Area("Student")]
    public class StudentNoteTypeController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public StudentNoteTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<NoteType> noteTypes = _unitOfWork.NoteType.GetAll();
            return View(noteTypes);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoteType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.NoteType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "New note type has been created.";
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

            var noteTypeFromDb = _unitOfWork.NoteType.GetFirstOrDefault(s => s.Id == id);

            if (noteTypeFromDb == null)
            {
                return NotFound();
            }

            return View(noteTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoteType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.NoteType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Note type updated.";
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

            var NoteTypeFromDb = _unitOfWork.NoteType.GetFirstOrDefault(s => s.Id == id);

            if (NoteTypeFromDb == null)
            {
                return NotFound();
            }

            return View(NoteTypeFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteNote(int? id)
        {
            var obj = _unitOfWork.NoteType.GetFirstOrDefault(s => s.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.NoteType.Remove(obj);

            _unitOfWork.Save();

            TempData["success"] = "Note Type has been deleted.";
            return RedirectToAction("Index");
        }
    }
}
