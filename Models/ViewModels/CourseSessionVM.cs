using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class CourseSessionVM
    {
        public CourseSession CourseSession { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CourseList { get; set; }

    }
}
