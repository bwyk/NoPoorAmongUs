using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class StudentVM
    {
        public Student Student { get; set; }
        public Guardian Guardian { get; set; }
        public Rating Rating { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StudentStatusList { get; set; }
    }
}
