using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using System.ComponentModel;

namespace Models.ViewModels
{
    public class NoteTypeVM
    {
        public NoteType NoteType { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> AllRoles { get; set; }
    }
}