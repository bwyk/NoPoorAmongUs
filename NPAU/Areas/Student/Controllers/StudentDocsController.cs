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

        public IActionResult Upsert(int? id)
        {
            StudentDocVM studentDocVm = new()
            {
                StudentDoc = new(),
                StudentDocTypeList = _unitOfWork.DocType.GetAll().Select(i => new SelectListItem  //Projection
                {
                    Text = i.TypeName,
                    Value = i.Id.ToString()
                })
            };

            return View(studentDocVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StudentDocVM obj)
        {
            obj.StudentDoc.DocType = _unitOfWork.DocType.GetFirstOrDefault(d => d.Id == obj.StudentDoc.DocTypeId);
            ModelState.Clear();
            TryValidateModel(obj);
            if (ModelState.IsValid)
            {
                if (obj.StudentDoc.Id == 0)
                {
                    _unitOfWork.StudentDoc.Add(obj.StudentDoc);
                }
                else
                {
                    _unitOfWork.StudentDoc.Update(obj.StudentDoc);
                }
                _unitOfWork.Save();
                TempData["success"] = "You have successfully added a new document.";
                return RedirectToAction("Index");
            }

            obj.StudentDocTypeList = _unitOfWork.DocType.GetAll().Select(i => new SelectListItem  //Projection
            {
                Text = i.TypeName,
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

            var studentDocFromDb = _unitOfWork.StudentDoc.GetFirstOrDefault(s => s.Id == id);

            if (studentDocFromDb == null)
            {
                return NotFound();
            }

            return View(studentDocFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDoc(int? id)
        {
            var obj = _unitOfWork.StudentDoc.GetFirstOrDefault(s => s.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.DocUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.StudentDoc.Remove(obj);

            _unitOfWork.Save();

            TempData["success"] = "Document has been deleted.";
            return RedirectToAction("Index");
        }
    }
}
