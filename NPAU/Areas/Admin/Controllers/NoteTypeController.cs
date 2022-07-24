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
            List<string> noteTypes = _unitOfWork.NoteType.GetAll().Select(n => n.Type).Distinct().ToList();
            Dictionary<string, List<string>> allData = new Dictionary<string, List<string>>();

            foreach (var nt in noteTypes)
            {
                List<string> rolesToAdd = new List<string>();
                rolesToAdd = _unitOfWork.NoteType.GetAll(u => u.Type == nt, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                allData.Add(nt, rolesToAdd);
            }

            NoteTypeVM noteTypeVM = new NoteTypeVM()
            {
                NoteRoles = allData
            };
            return View(noteTypeVM);
        }

        [HttpGet]
        public IActionResult Upsert(string? type)
        {
            NoteTypeVM noteTypeVM = new NoteTypeVM()
            {
                NoteType = new(),
                RoleList = _roleManager.Roles.Select(r => r.Name).ToList()
            };

            if (type == null)
            {
                return View(noteTypeVM);
            }
            else
            {
                List<string> noteTypes = _unitOfWork.NoteType.GetAll().Select(n => n.Type).Distinct().ToList();
                Dictionary<string, List<string>> allData = new Dictionary<string, List<string>>();

                foreach (var nt in noteTypes)
                {
                    List<string> rolesToAdd = new List<string>();
                    rolesToAdd = _unitOfWork.NoteType.GetAll(u => u.Type == nt, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                    allData.Add(nt, rolesToAdd);
                }

                noteTypeVM.NoteRoles = allData;
                noteTypeVM.NoteType = _unitOfWork.NoteType.GetFirstOrDefault(t => t.Type == type);
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
            List<NoteType> notetypes = new List<NoteType>();

            //New NoteType
            if (obj.NoteType.Id == 0)
            {
                foreach (var item in checkedRoles)
                {
                    obj.NoteType.Role = _roleManager.Roles.FirstOrDefault(r => r.Name == item);
                    NoteType nt = new NoteType();
                    nt.Type = obj.NoteType.Type;
                    nt.Role = obj.NoteType.Role;
                    notetypes.Add(nt);
                }

                _unitOfWork.NoteType.AddRange(notetypes);
                _unitOfWork.Save();
                TempData["success"] = "Note Type created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                //Update Notetype
                //Find what the currently saved Note Type Roles are.
                obj.RoleList = _unitOfWork.NoteType.GetAll(nt => nt.Type == obj.NoteType.Type, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                var oldTypeName = _unitOfWork.NoteType.GetFirstOrDefault(nt => nt.Id == obj.NoteType.Id, includeProperties: "Role");
                List<string> oldRoles = _unitOfWork.NoteType.GetAll(nt => nt.Type == oldTypeName.Type, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                oldRoles.Sort();
                checkedRoles.Sort();

                //Compare old checked roles to the new incoming checked roles.
                if (oldRoles.SequenceEqual(checkedRoles))
                {
                    //Roles didn't change.  Update NoteType Type
                    List<NoteType> noteTypeUpdates = _unitOfWork.NoteType.GetAll(nt => nt.Type == oldTypeName.Type).ToList();
                    foreach (NoteType nt in noteTypeUpdates)
                    {
                        nt.Type = obj.NoteType.Type;
                    }

                    _unitOfWork.NoteType.UpdateRange(noteTypeUpdates);
                    _unitOfWork.Save();
                    TempData["success"] = "Note Type editted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    var list1 = oldRoles.Except(checkedRoles); //removed
                    var list2 = checkedRoles.Except(oldRoles); //added
                    List<NoteType> rolesToRemove = new List<NoteType>();
                    List<NoteType> rolesToAdd = new List<NoteType>();
                    foreach (string s in oldRoles.Except(checkedRoles))
                    {
                        NoteType remove = _unitOfWork.NoteType.GetFirstOrDefault(nt => nt.Type == oldTypeName.Type && nt.Role.Name == s, includeProperties: "Role");
                        rolesToRemove.Add(remove);
                    }
                    foreach (string s in checkedRoles.Except(oldRoles))
                    {
                        NoteType add = new NoteType();
                        add.Type = obj.NoteType.Type;
                        add.Role = _roleManager.Roles.FirstOrDefault(r => r.Name == s);
                        rolesToAdd.Add(add);
                    }

                    _unitOfWork.NoteType.AddRange(rolesToAdd);
                    _unitOfWork.NoteType.RemoveRange(rolesToRemove);
                    _unitOfWork.Save();

                    TempData["success"] = "Note Type updated successfully";
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        public IActionResult Delete(string? type)
        {
            List<string> noteTypes = _unitOfWork.NoteType.GetAll().Select(n => n.Type).Distinct().ToList();
            Dictionary<string, List<string>> allData = new Dictionary<string, List<string>>();

            foreach (var nt in noteTypes)
            {
                List<string> rolesToAdd = new List<string>();
                rolesToAdd = _unitOfWork.NoteType.GetAll(u => u.Type == nt, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                allData.Add(nt, rolesToAdd);
            }

            NoteTypeVM noteTypeVM = new NoteTypeVM()
            {
                NoteRoles = allData,
                NoteType = _unitOfWork.NoteType.GetFirstOrDefault(t => t.Type == type),
                RoleList = _roleManager.Roles.Select(r => r.Name).ToList()
            };
            return View(noteTypeVM);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(NoteTypeVM obj)
        {
            NoteType incomingNT = _unitOfWork.NoteType.GetFirstOrDefault(nt => nt.Id == obj.NoteType.Id, includeProperties: "Role");
            IEnumerable<NoteType> foundNoteTypes = _unitOfWork.NoteType.GetAll(t => t.Type == incomingNT.Type);

            _unitOfWork.NoteType.RemoveRange(foundNoteTypes);
            _unitOfWork.Save();
            TempData["success"] = "Note Type deleted successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS 
        [HttpGet]
        public IActionResult GetAll()
        {
            var noteTypeList = _unitOfWork.NoteType.GetAll(includeProperties: "Role");
            return Json(new { data = noteTypeList });
        }

        #endregion
    }
}