using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.Academic.ViewModel
{
    public class StudentDocVM
    {
        public StudentDoc StudentDoc { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> StudentDocTypeList { get; set; }
    }
}
