using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.ViewModels
{

    public class GradingVM
    {

        public List<Grade> Grades { get; set; }
        
        public Assessment Assessment { get; set; }
        
        [ValidateNever]
        public List<CourseEnrollment> CourseEnrollmentList { get; set; }

        public List<Grade> AlreadyGradedList { get; set; }
        
        public List<StudentGradeVM> StudentGrades { get; set; }
    }
}
