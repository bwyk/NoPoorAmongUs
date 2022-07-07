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
    public class GradeVM
    {

        public Grade Grade { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CourseEnrollmentList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AssessmentList { get; set; }
    }
}
