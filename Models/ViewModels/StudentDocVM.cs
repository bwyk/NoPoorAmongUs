using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class StudentDocVM
    {
        public StudentDoc StudentDoc { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Students { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> DocTypeList { get; set; }
    }
}
