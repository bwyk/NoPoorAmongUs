using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using System.Diagnostics;

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
        public IActionResult Index()
        {
           return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            StudentDocVM studentDocVm = new()
            {
                StudentDoc = new(),
                Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                }),
                DocTypeList = _unitOfWork.DocType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.TypeName,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                return View(studentDocVm);
            }
            else
            {
                studentDocVm.StudentDoc = _unitOfWork.StudentDoc.GetFirstOrDefault(s => s.Id == id);
                return View(studentDocVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StudentDocVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"documents\students");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.StudentDoc.DocUrl != null)
                    {
                        var oldFilePath = Path.Combine(wwwRootPath, obj.StudentDoc.DocUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.StudentDoc.DocUrl = @"\documents\students\" + fileName + extension;
                }
            
                if (obj.StudentDoc.Id == 0)
                {
                    _unitOfWork.StudentDoc.Add(obj.StudentDoc);
                }
                else
                {
                    _unitOfWork.StudentDoc.Update(obj.StudentDoc);
                }
                _unitOfWork.Save();
                TempData["success"] = "Student Document added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                obj.Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
                {
                    Text = i.FirstName,
                    Value = i.Id.ToString()
                });

                obj.DocTypeList = _unitOfWork.DocType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.TypeName,
                    Value = i.Id.ToString()
                });

                return View(obj);
            }

        }

        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {
            var studentDocList = _unitOfWork.StudentDoc.GetAll(includeProperties: "Student,DocType");
            return Json(new { data = studentDocList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.StudentDoc.GetFirstOrDefault(s => s.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.DocUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.StudentDoc.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
