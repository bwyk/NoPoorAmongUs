using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Academic;
using Models.ViewModels;
using System.Collections.Generic;

namespace NPAU.Areas.Student.Controllers
{
    [Area("Student")]
    public class GradingController : Controller
    {
        [BindProperty]
        private GradingVM gradingVM { get; set; }
        [BindProperty]
        private StudentAssessmentGradesVM studentAssessmentGradesVM { get; set; }

        private readonly IUnitOfWork _unitOfWork;

        public GradingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult CourseSelect()
        {
            
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll(c => c.Term.IsActive == true);
            return View(courseList);
        }

        public IActionResult AssessmentSelect(int courseId)
        {
            IEnumerable<Assessment> assessmentList = _unitOfWork.Assessment.GetAll(a => a.CourseId == courseId);
            return View(assessmentList);
        }

        public IActionResult StudentSelect(int courseId)
        {
            List<CourseEnrollment> ceList = (List<CourseEnrollment>)_unitOfWork.CourseEnrollment.GetAll((c => c.CourseSession.CourseId == courseId), includeProperties: "Student");
            
            
          
            return View(ceList);
        }

        public IActionResult StudentAssessmentTable(int ceId)
        {
            studentAssessmentGradesVM = new StudentAssessmentGradesVM();
            CourseEnrollment cE = _unitOfWork.CourseEnrollment.GetFirstOrDefault((e => e.Id == ceId), includeProperties: "CourseSession,Student");

            studentAssessmentGradesVM.Student = cE.Student;
            
            studentAssessmentGradesVM.Course = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == cE.CourseSession.CourseId);
            
            studentAssessmentGradesVM.Assessments = (List<Assessment>)_unitOfWork.Assessment.GetAll(a => a.CourseId == studentAssessmentGradesVM.Course.Id);

            studentAssessmentGradesVM.Grades = new List<Grade>();

            foreach(Assessment a in studentAssessmentGradesVM.Assessments)
            {
                
                Grade gradeCheck = _unitOfWork.Grade.GetFirstOrDefault(g => g.AssessmentId == a.Id);
                Grade emptyGrade = new Grade();
                if (gradeCheck == null)
                {
                    
                    emptyGrade.Assessment = a;
                    emptyGrade.AssessmentId = a.Id;
                    emptyGrade.Id = 0;
                    emptyGrade.Comment = "Not Yet Graded";
                    emptyGrade.CourseEnrollment = cE;
                    emptyGrade.CourseEnrollmentId = cE.Id;
                    emptyGrade.Score = 0;
                    gradeCheck = emptyGrade;
                }
                
                studentAssessmentGradesVM.Grades.Add(gradeCheck);

            }

            return View(studentAssessmentGradesVM);
        }


        [HttpGet]
        public IActionResult GradingTable(int assessmentId)
        {
            //original attempt; KEEP OR NO?
            //Assessment targetAssessment = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == assessmentId);
            //GradingVM gradingVM = new GradingVM();           

            //IEnumerable<CourseEnrollment> ceList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetAssessment.CourseId), includeProperties: "Student");
            //foreach(CourseEnrollment cE in ceList)
            //{
            //    if (!gradingVM.StudentList.Contains(cE.Student)) {
            //        gradingVM.StudentList.Append(cE.Student);
            //    }
            //}
            //return View(gradingVM);

            Assessment targetAssessment = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == assessmentId);
            List<CourseEnrollment> ceList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetAssessment.CourseId), includeProperties: "Student").ToList();
            
            gradingVM = new GradingVM();
            gradingVM.Assessment = targetAssessment;
            gradingVM.CourseEnrollmentList = ceList;
            List<Grade> currentGrades = new List<Grade>();

            List<StudentGradeVM> studentGrades = new List<StudentGradeVM>();

            int maxScore = targetAssessment.MaxScore;
            foreach(CourseEnrollment cE in ceList)
            {
                int score = 0;
                int gradeId = -1;
                string comment = "";
                Models.Student student = cE.Student;

                Grade oldGrade = _unitOfWork.Grade.GetFirstOrDefault(g => g.CourseEnrollmentId == cE.Id && g.AssessmentId == targetAssessment.Id );

                if(oldGrade != null)
                {
                    score = oldGrade.Score;
                    gradeId = oldGrade.Id;
                    comment = oldGrade.Comment;
                }

                StudentGradeVM sg = new StudentGradeVM(student, score, maxScore, gradeId, comment);
                studentGrades.Add(sg);
                currentGrades.Append(oldGrade);
            }
            gradingVM.AlreadyGradedList = currentGrades;
            gradingVM.StudentGrades = studentGrades;
            return View(gradingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveGrades(GradingVM gradingVM)
        {
            //IEnumerable<Grade> olGrades = _unitOfWork.Assessment.GetAll(a => a.Id == gradingVM.Grades[0].AssessmentId);
            //foreach (CourseEnrollment cE in gradingVM.CourseEnrollmentList)
            //{
            foreach (Grade g in gradingVM.Grades)
            {
                g.CourseEnrollment = _unitOfWork.CourseEnrollment.GetFirstOrDefault(ce => ce.Id == g.CourseEnrollment.Id);
                g.Assessment = _unitOfWork.Assessment.GetFirstOrDefault(a => a.Id == g.Assessment.Id);
                if (g.Id == -1)
                {
                    Grade newGrade = new Grade()
                    {
                        CourseEnrollmentId = g.CourseEnrollment.Id,
                        CourseEnrollment = g.CourseEnrollment,
                        Score = g.Score,
                        Assessment = g.Assessment,
                        AssessmentId = g.Assessment.Id,
                        Comment = g.Comment
                    };
                    _unitOfWork.Grade.Add(newGrade);
                    _unitOfWork.Save();
                    //newGrade = _unitOfWork.Grade.GetFirstOrDefault(gr => (gr.AssessmentId == sgvm..AssessmentId) && (gr.CourseEnrollmentId == g.CourseEnrollmentId));
                }
                else
                {
                    _unitOfWork.Grade.Update(g);
                    _unitOfWork.Save();
                }

            }
            TempData["success"] = "All grades saved successfully.";

            return RedirectToAction("CourseSelect");
        }

        #region API CALLS
        public IActionResult GetAll()
        {
            var gradingList = _unitOfWork.Grade.GetAll(includeProperties: "Assessment,CourseEnrollment,Course");
            return Json(new { data = gradingList });
        }
        #endregion
    }


}
