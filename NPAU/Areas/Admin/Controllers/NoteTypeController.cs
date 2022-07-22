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
        public ViewResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            NoteTypeVM noteTypeVM = new NoteTypeVM()
            {
                NoteType = new(),
                RoleList = _roleManager.Roles.Select(r => r.Name).ToList()
            };

            if (id == null || id == 0)
            {
                return View(noteTypeVM);
            }
            else
            {
                noteTypeVM.NoteType = _unitOfWork.NoteType.GetFirstOrDefault(u => u.Id == id, includeProperties: "Role");
                noteTypeVM.RoleList = _roleManager.Roles.Select(r => r.Name).ToList();

                return View(noteTypeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(NoteTypeVM obj)
        {
            var checkedRoles = Request.Form["roles"].ToList();
            var roleIds = new List<string>();

            foreach(var item in checkedRoles)
            {
                obj.NoteType.Role = _roleManager.Roles.FirstOrDefault(r => r.Name == item);
                _unitOfWork.NoteType.Add(obj.NoteType);
                TempData["success"] = "Note Type Created Successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");

/*            obj.RoleList = _roleManager.Roles.Select(r => r.Name).ToList();

            return View(obj);*/

            //            var oldNotesRoles = _unitOfWork.NoteType.GetAll().Where(n => n.RoleId == obj.NoteType.RoleId).ToList();



            /*if (ModelState.IsValid)
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
            }*/

            obj.RoleList = _roleManager.Roles.Select(r => r.Name).ToList();

            return View(obj);
        }

        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            var noteTypeList = _unitOfWork.NoteType.GetAll(includeProperties: "Role");
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