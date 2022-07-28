using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace Models.ViewModels
{
    public class NotesVM
    {
        public StudentNote StudentNote { get; set; }

        [ValidateNever]
        public ApplicationUser AppUser { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Students { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> PriorityList { get; set; }
    }
}
