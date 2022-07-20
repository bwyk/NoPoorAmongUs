using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using Models.ViewModels;
using System.Diagnostics;
using Utilities;

namespace NPAU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoteTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;

        public NoteTypeController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            NoteTypeVM noteTypeVM = new NoteTypeVM()
            {
                NoteType = new(),
                AllRoles = _roleManager.Roles.ToList().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                return View(noteTypeVM);
            }
            else
            {
                noteTypeVM.NoteType = _unitOfWork.NoteType.GetFirstOrDefault(u => u.Id == id);
                return View(noteTypeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(NoteTypeVM obj)
        {
            if (ModelState.IsValid)
            {  
                if (obj.NoteType.Id == 0)
                {
                    _unitOfWork.NoteType.Add(obj.NoteType);
                    TempData["success"] = "Note Type Created Successfully";

                }
                else
                {
                    _unitOfWork.NoteType.Update(obj.NoteType);
                    TempData["success"] = "Note Type Updated Successfully";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            obj.AllRoles = _roleManager.Roles.ToList().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

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