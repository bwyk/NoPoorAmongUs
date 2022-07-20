using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using Models.ViewModels;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            NotesVM notesVM = new()
            {
                StudentNote = new(),
                Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem  //Projection
                {
                    Text = i.FullName,
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

            if (ModelState.IsValid)
            {
                if (obj.StudentNote.Id == 0)
                {
                    obj.StudentNote.CreatedDate = DateTime.Now;
                    _unitOfWork.StudentNote.Add(obj.StudentNote);
                    TempData["success"] = "A new note has been created.";
                }
                else
                {
                    _unitOfWork.StudentNote.Update(obj.StudentNote);
                    TempData["success"] = "A note has been modified.";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            obj.Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Id.ToString()
            });

            obj.NoteTypeList = _unitOfWork.NoteType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Type,
                Value = i.Id.ToString()
            });

            return View(obj);
        }

        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {
            var studentNoteList = _unitOfWork.StudentNote.GetAll(includeProperties: "Student,NoteType");
            return Json(new { data = studentNoteList });
        }

        [HttpGet]
        public IActionResult GetNoteText(int? id)
        {
            var noteText = _unitOfWork.StudentNote.GetFirstOrDefault(note => note.Id == id);
            return Json(new { data = noteText });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.StudentNote.GetFirstOrDefault(s => s.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.StudentNote.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
