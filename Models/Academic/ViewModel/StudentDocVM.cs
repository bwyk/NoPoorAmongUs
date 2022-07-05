using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Academic.ViewModel
{
    public class StudentDocVM
    {
        public StudentDoc StudentDoc { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> StudentDocTypeList { get; set; }
    }
}
