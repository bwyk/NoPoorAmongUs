using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.Academic;
using Models.ViewModels;
using System.Diagnostics;
using Utilities;

namespace NPAU.Controllers
{
    [Area("Student")]
    public class StudentNotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;


        public StudentNotesController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ViewResult> Upsert(int? id)
        {
            //Need to check what Role I'm logged in as.
            string userID = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID);
            //Find out this user's Roles Name.
            var userRoleNames = await _userManager.GetRolesAsync(applicationUser);
            //Grab all the NoteTypes Role Names
            List<NoteType> allNoteTypes = _unitOfWork.NoteType.GetAll(includeProperties: "Role").ToList();
            List<NoteType> allowedNoteTypes = new List<NoteType>();

            List<IdentityRole> userRoles = new List<IdentityRole>();

            foreach(string roleName in userRoleNames)
            {
                userRoles.Add(await _roleManager.FindByNameAsync(roleName));
            }

            List<string> noteTypes = _unitOfWork.NoteType.GetAll().Select(n => n.Type).Distinct().ToList();
            Dictionary<string, List<string>> allData = new Dictionary<string, List<string>>();

            foreach (var nt in noteTypes)
            {
                List<string> rolesToAdd = new List<string>();
                rolesToAdd = _unitOfWork.NoteType.GetAll(u => u.Type == nt, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                allData.Add(nt, rolesToAdd);
            }

            foreach (KeyValuePair<string, List<string>> entry in allData)
            {
                foreach (string noteType in entry.Value)
                {
                    if(userRoleNames.Contains(noteType))
                    {
                        Debug.WriteLine("Need to Add: " + entry.Key);
                        allowedNoteTypes.Add(_unitOfWork.NoteType.GetFirstOrDefault(nt => nt.Type == entry.Key, includeProperties: "Role"));
                    }
                }
            }
            
            NotesVM notesVM = new()
            {
                StudentNote = new(),
                Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                }),
                NoteTypeList = allowedNoteTypes.Select(i => new SelectListItem
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
