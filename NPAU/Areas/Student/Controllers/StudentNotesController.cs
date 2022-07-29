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
        public async Task<IActionResult> Index()
        {
            //Need to check what Role I'm logged in as.
            string userID = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID);

            var allowedNoteTypes = await GetAllowedNoteTypes();

            //Create Student Notes Viewmodel
            NotesVM notesVM = new()
            {
                StudentNote = new(),
                AppUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID),
                NoteTypeList = allowedNoteTypes.Select(i => new SelectListItem
                {
                    Text = i.Type,
                    Value = i.Id.ToString()
                })
            };

            return View(notesVM);
        }

        public async Task<List<NoteType>> GetAllowedNoteTypes()
        {
            //Need to check what Role I'm logged in as.
            string userID = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID);
            //Find out this user's Role Names.
            var userRoleNames = await _userManager.GetRolesAsync(applicationUser);
            //Creat empty list we can add allowed NoteTypes to.
            List<NoteType> allowedNoteTypes = new List<NoteType>();
            //Grab list of all current NoteType Types in db.
            List<string> noteTypes = _unitOfWork.NoteType.GetAll().Select(n => n.Type).Distinct().ToList();
            //Create emppty dictionary with notetype type as the key and the roles that can access as the values.
            Dictionary<string, List<string>> allData = new Dictionary<string, List<string>>();

            //Loop through all the notetype types, on each grab the roles associated with the type.  Add them to dictionary.
            foreach (var nt in noteTypes)
            {
                List<string> rolesToAdd = new List<string>();
                rolesToAdd = _unitOfWork.NoteType.GetAll(u => u.Type == nt, includeProperties: "Role").Select(n => n.Role.Name).ToList();
                allData.Add(nt, rolesToAdd);
            }

            //Loop through dictionary and check what this User has access.  Add allowable notetype if they have access.
            foreach (KeyValuePair<string, List<string>> entry in allData)
            {
                foreach (string noteType in entry.Value)
                {
                    if (userRoleNames.Contains(noteType))
                    {
                        allowedNoteTypes.Add(_unitOfWork.NoteType.GetFirstOrDefault(nt => nt.Type == entry.Key, includeProperties: "Role"));
                    }
                }
            }
            allowedNoteTypes.Sort((p, q) => p.Type.CompareTo(q.Type));
            return allowedNoteTypes.Distinct().ToList();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            // Need to check what Role I'm logged in as and grab the allowed NoteTypes.
            string userID = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            List<NoteType> allowedNoteTypes = await GetAllowedNoteTypes();

            //Create Student Notes Viewmodel
            NotesVM notesVM = new()
            {
                StudentNote = new(),
                AppUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID),
                Students = _unitOfWork.Student.GetAll().Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                }),
                NoteTypeList = allowedNoteTypes.Select(i => new SelectListItem
                {
                    Text = i.Type,
                    Value = i.Id.ToString()
                }),
                PriorityList = SD.PriorityList.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),
            };

            if (id == null || id == 0)
            {
                return View(notesVM);
            }
            else
            {
                notesVM.StudentNote = _unitOfWork.StudentNote.GetFirstOrDefault(u => u.Id == id, "Student,NoteType,ApplicationUser");
                return View(notesVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(NotesVM obj)
        {
            string userID = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            obj.StudentNote.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userID);

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

            obj.PriorityList = SD.PriorityList.Select(i => new SelectListItem
            {
                Text = i,
                Value = i
            });

            return View(obj);
        }

        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {
            var studentNoteList = _unitOfWork.StudentNote.GetAll(includeProperties: "Student,NoteType,ApplicationUser");
            return Json(new { data = studentNoteList });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByRole()
        {
            List<NoteType> allowedNoteTypes = await GetAllowedNoteTypes();
            List<StudentNote> studentNoteList = new List<StudentNote>();

            foreach(var notetype in allowedNoteTypes)
            {
                var results = _unitOfWork.StudentNote.GetAll(s => s.NoteType.Type == notetype.Type, includeProperties: "Student,NoteType,ApplicationUser");
                studentNoteList.AddRange(results);
            }
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
