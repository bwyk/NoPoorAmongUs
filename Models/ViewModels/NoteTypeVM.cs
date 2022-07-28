using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class NoteTypeVM
    {
        public NoteType NoteType { get; set; }

        public IEnumerable<NoteType> NoteTypes { get; set;}

        [ValidateNever]
        public Dictionary<string, List<string>> NoteRoles { get; set; }

        [ValidateNever]
        public List<string> RoleList { get; set; }
    }
}