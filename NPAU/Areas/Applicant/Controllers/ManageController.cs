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

namespace NPAU.Controllers
{
    [Area("Applicant")]
    public class ManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public StudentVM StudentVM { get; set; } = null!;
        public ManageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult Upsert(StudentVM obj)
        {
            if (ModelState.IsValid)
            {
                bool match;
                bool saveChanges = false;
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


        //var guardianIds = relationships.Select(relationships => relationships.GuardianId).Contains(global.Id);
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
        public void SaveRatings(Rating rating)
        {
            string result;
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
        public IActionResult GetAll(string status)
        {
            IEnumerable<Student> students;
            students = _unitOfWork.Student.GetAll();
   

            switch (status)
            {
                case "pending":
                    students = students.Where(s => s.Status == SD.StudentStatusPending);
                    break;
                case "student":
                    students = students.Where(s => s.Status == SD.StudentStatusAccepted);
                    break;
                case "rejected":
                    students = students.Where(s => s.Status == SD.StudentStatusRejected);
                    break;
                default:
                    break;
            }

            return Json(new { data = students });
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
