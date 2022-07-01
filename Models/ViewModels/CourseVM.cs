using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering; //REQUIRES NUGET Microsoft.AspNetCore.Mvc.View


namespace Models.ViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SchoolList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> InstructorList { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> SubjectList { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> TermList { get; set; }

    }
}
