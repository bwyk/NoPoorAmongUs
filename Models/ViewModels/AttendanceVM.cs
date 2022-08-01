using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace Models.ViewModels
{
    public class AttendanceVM
    {
        //public Attendance Attendance { get; set; }
        [BindProperty]
        public SessionAttendance SessionAttendance { get; set; }
        [ValidateNever]
        public List<Attendance> AttendanceList { get; set; }
        //public List<CourseEnrollment> CourseEnrollmentList { get; set; }



    }
}
