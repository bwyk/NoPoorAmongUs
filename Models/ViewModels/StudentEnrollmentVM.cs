using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace Models.ViewModels
{
    public class StudentEnrollmentVM
    {
        public CourseSession courseSession { get; set; }
        [ValidateNever]
        public List<Student> Student { get; set; }
    }
}
