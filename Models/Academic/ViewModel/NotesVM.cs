using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.Academic.ViewModel
{
    public class NotesVM
    {
        public StudentNote StudentNote { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> StudentList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }
    }
}
