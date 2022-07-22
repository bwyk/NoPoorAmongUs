using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace Models.ViewModels
{
    public class AttendanceVM
    {
        public Attendance Attendance { get; set; }
        [ValidateNever]
        public List<CourseEnrollment> CourseEnrollmentList { get; set; }



    }
}
