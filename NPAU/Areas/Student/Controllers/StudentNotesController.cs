using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.Academic;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentNotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentNotesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int id)
        {
            IEnumerable<StudentNote> objStudentNoteList = _unitOfWork.StudentNote.GetAll().Where(u => u.StudentId == id);

            return View(objStudentNoteList);
        }

        [HttpGet]
        public ViewResult CreateNote(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNote(StudentNote obj, int id)
        {
            if (ModelState.IsValid)
            {
                obj.StudentId = id;
                _unitOfWork.StudentNote.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "New note has been created.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditNote(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var studentNoteFromDb = _unitOfWork.StudentNote.GetFirstOrDefault(s => s.Id == id);

            if(studentNoteFromDb == null)
            {
                return NotFound();
            }

            return View(studentNoteFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNote(StudentNote obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.StudentNote.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Note has been updated.";
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

            var studentNoteFromDb = _unitOfWork.StudentNote.GetFirstOrDefault(s => s.Id == id);

            if (studentNoteFromDb == null)
            {
                return NotFound();
            }

            return View(studentNoteFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteNote(int? id)
        {
            var obj = _unitOfWork.StudentNote.GetFirstOrDefault(s => s.Id == id);
            if(obj == null)
            {
                return NotFound();
            }

            _unitOfWork.StudentNote.Remove(obj);

            _unitOfWork.Save();

            TempData["success"] = "Note has been deleted.";
            return RedirectToAction("Index");
        }
    }
}
