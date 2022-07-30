using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Models;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.People;
using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class ManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public StudentVM StudentVM { get; set; } = null!;
        public ManageController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Upsert(int? id)
        {
            StudentVM studentVM = new()
            {
                StudentStatusList = SD.StudentStatusList,
                GuardianRelationshipList = SD.GuardianRelationshipList
            };

            if (id == null || id == 0)
            {
                studentVM.Student = new();
                return View(studentVM);
            }
            else
            {
                studentVM.Student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);
                studentVM.Rating = _unitOfWork.Rating.GetFirstOrDefault(r => r.StudentId == id);
                return View(studentVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.Rating.StudentId = obj.Student.Id;
                _unitOfWork.Rating.Add(obj.Rating);
                _unitOfWork.Save();

                TempData["success"] = "Ratings added successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                bool match;
                var enteredGuardians = new List<Guardian>();
                if (!string.IsNullOrEmpty(obj.GuardianJSON))
                {
                    enteredGuardians = JsonConvert.DeserializeObject<List<Guardian>>(obj.GuardianJSON);
                }
                // If new student/applicant
                if (obj.Student.Id == 0)
                {
                    _unitOfWork.Student.Add(obj.Student);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Student.Update(obj.Student);
                    _unitOfWork.Save();
                }

                Student student = _unitOfWork.Student.GetFirstOrDefault(student => (student.FirstName == obj.Student.FirstName) &&
                                                                (student.LastName == obj.Student.LastName));

                // Get all relationships involving the student/applicant
                var relationships = _unitOfWork.Relationship.GetAll(r => r.StudentId == obj.Student.Id);
                List<Guardian> newGuardians = new();
                // Check for existing or new relationships
                foreach (Guardian g in enteredGuardians ?? Enumerable.Empty<object>()) // If null do nothing
                {
                    match = false;
                    // If this is a new guardian
                    if (g.Id == -1)
                    {
                        _unitOfWork.Guardian.Add(new Guardian()
                        {
                            FirstName = g.FirstName,
                            LastName = g.LastName,
                            Relationship = g.Relationship
                        });
                        _unitOfWork.Save();
                    }

                    // check for any existing student guardian relationships
                    foreach (var relationship in relationships)
                        if (relationship.GuardianId == g.Id)
                        {
                            match = true;
                            break;
                        }

                    // If there are none make a new relationship between guardian and student
                    if (!match)
                    {
                        Guardian guardian = _unitOfWork.Guardian.GetFirstOrDefault(guardian => (guardian.FirstName == g.FirstName) && guardian.LastName == g.LastName);
                        _unitOfWork.Relationship.Add(new Relationship()
                        {
                            Student = student,
                            StudentId = student.Id,
                            RelationshipType = g.Relationship,
                            Guardian = guardian,
                            GuardianId = guardian.Id
                        });
                        _unitOfWork.Save();
                        newGuardians.Add(guardian);
                        relationships = _unitOfWork.Relationship.GetAll(r => r.StudentId == obj.Student.Id);
                    }
                }


                // Check if any relationships were removed
                foreach (Relationship r in relationships)
                    // If there are not any entered guardians that match an existing relationship
                    if (!enteredGuardians.Any(g => g.Id == r.GuardianId))
                    {
                        if (!newGuardians.Any(g => g.Id == r.GuardianId))
                            _unitOfWork.Relationship.Remove(r);
                    }

                
                TempData["success"] = "Applicant updated successfully!";


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            obj.PotentialGuardians = _unitOfWork.Guardian.GetAll();
            return View(obj);
        }

        private IEnumerable<Guardian> GetCurrentGuardians(int studentId)
        {
            // All relationships involving the student
            var relationships = _unitOfWork.Relationship.GetAll(r => r.StudentId == studentId);
            // All guardians that are included in the student relationships
            var guardians = _unitOfWork.Guardian.GetAll(g => relationships.Select(relationships => relationships.GuardianId).Contains(g.Id));

            List<Guardian> assembledGuardians = new();
            foreach (Relationship r in relationships)
            {
                var guardian = guardians.FirstOrDefault(g => g.Id == r.GuardianId);
                if (guardian != null)
                {
                    guardian.Relationship = r.RelationshipType;
                    assembledGuardians.Add(guardian);
                }
            }

            return assembledGuardians;
        }

        private IEnumerable<Guardian> GetPotentialGuardians(int studentId)
        {
            // All relationships involving the student
            var relationships = _unitOfWork.Relationship.GetAll(r => r.StudentId == studentId);
            // All guardians that are NOT included in the student relationships
            var guardians = _unitOfWork.Guardian.GetAll(g => !relationships.Select(relationships => relationships.GuardianId).Contains(g.Id));
            return guardians;
        }

        private Rating GetCurrentRating(int studentId)
        {
            var rating = _unitOfWork.Rating.GetFirstOrDefault(r => r.StudentId == studentId);
            if (rating == null)
            {
                rating = new Rating();
            }
            return rating;
        }

        [HttpPost, ActionName("SaveRatings")]
        public IActionResult SaveRatings(Rating rating, int? studentId)
        {
            string result;
            rating.ApplicationUserId = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            if (rating.Id == 0)
            {
                //Rating newRating = new Rating()
                //{
                //    Age = rating.Age,
                //    Academics = rating.Academics,
                //    AnnualIncome = rating.AnnualIncome,
                //    FamilySupport = rating.FamilySupport,
                //    Distance = rating.Distance
                //};
                _unitOfWork.Rating.Add(rating);
                result = "saved";
            }
            else
            {
                _unitOfWork.Rating.Update(rating);
                result = "updated";
            }
            _unitOfWork.Save();
            TempData["success"] = "Rating " + result + " successfully";            
            return Json(new { success = true});
        }
        
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Student.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Student deleted successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var studentFromDb = _unitOfWork.Student.GetFirstOrDefault(c => c.Id == id);

            if (studentFromDb == null)
            {
                return NotFound();
            }

            return View(studentFromDb);
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Instructor + "," + SD.Role_Social + "," + SD.Role_Rater)]
        public IActionResult GetAll(string status)
        {
            string userId = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
            string[] args = new string[0];
            string role = "";
            string filter1;
            if (status != null)
            {
                args = status.Split('_');
                role = args[0];
                
            }
            
            IEnumerable<Rating> studentRatings;
            IEnumerable<Student> students;
            students = _unitOfWork.Student.GetAll();
            switch (role)
            {
                case SD.Role_Instructor:
                    if (User.IsInRole(SD.Role_Instructor) || User.IsInRole(SD.Role_Admin))
                    {
                        filter1 = args[1];
                    switch (filter1)
                    {
                        case "all":
                            students = _unitOfWork.Student.GetAll(s => s.Status == SD.StudentStatusAccepted);
                            break;
                        case "your":
                            //students.Where(s => !studentRatings.Select(rating => rating.StudentId).Contains(s.Id));
                            var course = _unitOfWork.Course.GetAll(c => c.InstructorId == userId);// TODO change to user id

                            var sessions = _unitOfWork.CourseSession.GetAll(s => course.Select(c => c.Id).Contains(s.CourseId));
                            var enrollments = _unitOfWork.CourseEnrollment.GetAll(e => sessions.Select(s => s.Id).Contains(e.CourseSessionId));
                            students = _unitOfWork.Student.GetAll(s => enrollments.Select(e => e.StudentId).Contains(s.Id));

                            break;
                    }
                    
                    return Json(new { data = students });
                    }
                    break;
                case SD.Role_Social:
                    if (User.IsInRole(SD.Role_Social) || User.IsInRole(SD.Role_Admin))
                    {
                        filter1 = args[1];
                    switch (filter1)
                    {
                        case "all":
                            students = _unitOfWork.Student.GetAll();
                            break;
                        case "pending":
                            students = students.Where(s => s.Status == SD.StudentStatusPending);
                            break;
                        case "students":
                            students = students.Where(s => s.Status == SD.StudentStatusAccepted);
                            break;
                        case "rejected":
                            students = students.Where(s => s.Status == SD.StudentStatusRejected);
                            break;
                    }
                    return Json(new { data = students });
                    }
                    break;
                case SD.Role_Rater:
                    if (User.IsInRole(SD.Role_Rater) || User.IsInRole(SD.Role_Admin))
                    {
                        string raterId = ClaimsPrincipalExtensions.GetLoggedInUserId<string>(User);
                        studentRatings = _unitOfWork.Rating.GetAll(r => r.ApplicationUserId == raterId);
                        filter1 = args[2];
                        switch (filter1)
                        {
                            case "incomplete":
                                students = students.Where(s => !studentRatings.Select(rating => rating.StudentId).Contains(s.Id));
                                break;
                            case "complete":
                                students = students.Where(s => studentRatings.Select(rating => rating.StudentId).Contains(s.Id));
                                break;
                        }
                        return Json(new { data = students });
                    }
                    break;
            }

            return Json(new { data = "" });
        }
        [HttpGet]
        public IActionResult GetPotentialGuardians(string status, int studentId)
        {
            IEnumerable<Guardian> guardians;
            guardians = GetPotentialGuardians(studentId);

            return Json(new { data = guardians });
        }
        [HttpGet]
        public IActionResult GetCurrentGuardians(string status, int studentId)
        {
            IEnumerable<Guardian> guardians;
            guardians = GetCurrentGuardians(studentId);

            return Json(new { data = guardians });
        }
    }
}
