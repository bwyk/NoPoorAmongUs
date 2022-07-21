using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.People;

namespace Models.ViewModels
{
    public class StudentVM
    {
        public Student Student { get; set; }
        [ValidateNever]
        public string? GuardianJSON { get; set; }
        [ValidateNever]
        public Guardian Guardian { get; set; }
        [ValidateNever]
        public Rating Rating { get; set; }
        [ValidateNever]
        public Relationship Relationship { get; set; }
        [ValidateNever]
        public IEnumerable<Guardian> Guardians { get; set; }
        [ValidateNever]
        public IEnumerable<Guardian>? PotentialGuardians { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StudentStatusList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GuardianRelationshipList { get; set; }
    }
}
