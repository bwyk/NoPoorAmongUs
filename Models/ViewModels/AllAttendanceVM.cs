using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Academic;

namespace Models.ViewModels
{
    public class AllAttendanceVM
    {
        //[BindProperty]
        public List<AttendanceVM> AllSessionAttendance { get; set; }

    }
}
