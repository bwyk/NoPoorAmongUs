using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AssessmentVM
    {
        public Assessment Assessment { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CourseList { get; set; }
    }
}
