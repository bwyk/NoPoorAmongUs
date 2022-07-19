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

        private readonly IUnitOfWork _unitOfWork;

        public GradingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult CourseSelect()
        {
            IEnumerable<Course> courseList = _unitOfWork.Course.GetAll();
            return View(courseList);
        }

        public IActionResult AssessmentSelect(int courseId)
        {
            IEnumerable<Assessment> assessmentList = _unitOfWork.Assessment.GetAll(a => a.CourseId == courseId);
            return View(assessmentList);
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
                Models.Student student = cE.Student;

                Grade oldGrade = _unitOfWork.Grade.GetFirstOrDefault(g => g.CourseEnrollmentId == cE.Id );

                if(oldGrade != null)
                {
                    score = oldGrade.Score;
                    gradeId = oldGrade.Id;
                }

                StudentGradeVM sg = new StudentGradeVM(student, score, maxScore, gradeId);
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
                        AssessmentId = g.Assessment.Id
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
