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

        public Grade Grade { get; set; }
        
        public Assessment Assessment { get; set; }
        
        [ValidateNever]
        public IEnumerable<CourseEnrollment> CourseEnrollmentList { get; set; }

        public IEnumerable<Grade> AlreadyGradedList { get; set; }
        
        
    }
}
