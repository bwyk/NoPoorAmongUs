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
            IEnumerable<CourseEnrollment> ceList = _unitOfWork.CourseEnrollment.GetAll((cE => cE.CourseSession.CourseId == targetAssessment.CourseId), includeProperties: "Student");
            
            GradingVM gradingVM = new GradingVM();
            gradingVM.Assessment = targetAssessment;
            gradingVM.CourseEnrollmentList = ceList;
            IEnumerable<Grade> currentGrades = new List<Grade>();

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
        public IActionResult SaveGrades(List<Grade> gradeList)
        {
            
            foreach (Grade g in gradeList)
            {
                Grade changedGrade = _unitOfWork.Grade.GetFirstOrDefault(gr => (gr.AssessmentId == g.AssessmentId) && (gr.CourseEnrollmentId == g.CourseEnrollmentId));

                if (changedGrade == null) //if the grade isn't in the list...
                {
                    Grade newGrade = new Grade();
                    newGrade.Score = g.Score;
                    newGrade.Assessment = g.Assessment;
                    newGrade.AssessmentId = g.AssessmentId;
                    newGrade.CourseEnrollmentId = g.CourseEnrollmentId;
                    newGrade.CourseEnrollment = g.CourseEnrollment;
                    _unitOfWork.Grade.Add(newGrade);
                    _unitOfWork.Save();
                } else //if the grade IS in the list
                {
                    changedGrade.Score = g.Score;
                    _unitOfWork.Grade.Update(changedGrade);
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
