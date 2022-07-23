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
            //This currently works to fill the html with type and list of roles.  They are strings though.
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
            List<NoteType> notetypes = new List<NoteType>();

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
            return RedirectToAction("Index");
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