using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.Academic;
using Models.ViewModels;
using Utilities;

namespace NPAU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoteTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            NoteType noteType = new();

            if (id == null || id == 0)
            {
                return View(noteType);
            }
            else
            {
                noteType= _unitOfWork.NoteType.GetFirstOrDefault(u => u.Id == id);
                return View(noteType);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(NoteType obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.NoteType.Add(obj);
                    TempData["success"] = "Note Type Created Successfully";

                }
                else
                {
                    _unitOfWork.NoteType.Update(obj);
                    TempData["success"] = "Note Type Updated Successfully";

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
            var noteTypeList = _unitOfWork.NoteType.GetAll();
            return Json(new { data = noteTypeList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.NoteType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.NoteType.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Note Type Deleted Successfully" });

        }

        #endregion
    }
}
