using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Utilities;

namespace NPAU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            DocType docType = new();

            if (id == null || id == 0)
            {
                return View(docType);
            }
            else
            {
                docType= _unitOfWork.DocType.GetFirstOrDefault(u => u.Id == id);
                return View(docType);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(DocType obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.DocType.Add(obj);
                    TempData["success"] = "Doctype Created Successfully";

                }
                else
                {
                    _unitOfWork.DocType.Update(obj);
                    TempData["success"] = "Doctype Updated Successfully";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            var docTypeList = _unitOfWork.DocType.GetAll();
            return Json(new { data = docTypeList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.DocType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.DocType.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Doctype Deleted Successfully" });

        }

        #endregion
    }
}
