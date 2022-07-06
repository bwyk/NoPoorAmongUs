using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using Models.Academic.ViewModel;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentDocsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentDocsController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(int id)
        {
            IEnumerable<StudentDoc> objStudentDocList = _unitOfWork.StudentDoc.GetAll().Where(u => u.StudentId == id);

            return View(objStudentDocList);
        }

        [HttpGet]
        public ViewResult CreateDoc(int id)
        {
            StudentDocVM studentDocVm = new()
            {
                StudentDoc = new(),
                StudentDocTypeList = _unitOfWork.DocType.GetAll().Select(i => new SelectListItem  //Projection
                {
                    Text = i.Type,
                    Value = i.Id.ToString()
                })
            };
            return View(studentDocVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDoc(StudentDocVM obj, int id, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    //Don't forget to physically create these folders in wwwroot
                    var uploads = Path.Combine(wwwRootPath, @"images\docs");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.StudentDoc.DocUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.StudentDoc.DocUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.StudentDoc.DocUrl = @"\images\docs\" + fileName + extension;

                }
                obj.StudentDoc.StudentId = id;
                _unitOfWork.StudentDoc.Add(obj.StudentDoc);
                _unitOfWork.Save();
                TempData["success"] = "New Document has been added.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult EditDoc(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDoc(StudentDoc obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.StudentDoc.Update(obj);
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
            if (obj == null)
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
