using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.People;
using Models.Academic;

namespace Models.ViewModels
{
    public class SessionVM
    {
        public CourseSession CourseSession { get; set; }
        [ValidateNever]
        public string? CourseEnrollmentStudentsJSON { get; set; }
        [ValidateNever]
        public IEnumerable<Student>? PotentialStudents { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? Courses { get; set; }
    }
}
