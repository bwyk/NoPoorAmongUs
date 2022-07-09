using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using Models.Academic.ViewModel;

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
            //IEnumerable<StudentNote> objStudentNoteList = _unitOfWork.StudentNote.GetAll().Where(u => u.StudentId == id);
            IEnumerable<StudentNote> objStudentNoteList = _unitOfWork.StudentNote.GetAll();

            return View(objStudentNoteList);
        }

        public IActionResult Upsert(int? id)
        {
            NotesVM notesVM = new()
            {
                StudentNote = new(),
                StudentList = _unitOfWork.Student.GetAll().Select(i => new SelectListItem  //Projection
                {
                    Text = i.FirstName + i.LastName,
                    Value = i.Id.ToString()
                }),
                NoteTypeList = _unitOfWork.NoteType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Type,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(notesVM);
            }
            else 
            {
                notesVM.StudentNote = _unitOfWork.StudentNote.GetFirstOrDefault(u => u.Id == id);
                return View(notesVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(NotesVM obj)
        {
            obj.StudentNote.NoteType = _unitOfWork.NoteType.GetFirstOrDefault(n => n.Id == obj.StudentNote.NoteTypeId);
            obj.StudentNote.Student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == obj.StudentNote.StudentId);
            ModelState.Clear();
            TryValidateModel(obj);
            if (ModelState.IsValid)
            {
                if (obj.StudentNote.Id == 0)
                {
                    _unitOfWork.StudentNote.Add(obj.StudentNote);
                }
                else
                {
                    _unitOfWork.StudentNote.Update(obj.StudentNote);
                }
                _unitOfWork.Save();
                TempData["success"] = "A new note has been created.";
                return RedirectToAction("Index");
            }

            obj.StudentList = _unitOfWork.Student.GetAll().Select(i => new SelectListItem  //Projection
            {
                Text = i.FirstName + i.LastName,
                Value = i.Id.ToString()
            });

            obj.NoteTypeList = _unitOfWork.NoteType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Type,
                Value = i.Id.ToString()
            });

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
