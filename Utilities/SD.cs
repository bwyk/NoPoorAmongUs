using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class SD
    {
        public enum Weekdays
        {
            Monday = 0,
            Tuesday = 1,
            Wednesday = 2,
            Thursday = 3,
            Friday = 4
        }

        public const string StudentStatusApplicant = "Applicant";
        public const string StudentStatusRejected = "Rejected";
        public const string StudentStatusAccepted = "Student";
        public const string StudentStatusPending = "Pending";


        private static SelectListItem pending = new SelectListItem(SD.StudentStatusPending, SD.StudentStatusPending);
        private static SelectListItem accepted = new SelectListItem(SD.StudentStatusAccepted, SD.StudentStatusAccepted);
        private static SelectListItem rejected = new SelectListItem(SD.StudentStatusRejected, SD.StudentStatusRejected);
        public static IEnumerable<SelectListItem> StudentStatusList = new List<SelectListItem>() { pending, accepted, rejected };

        private static SelectListItem all = new SelectListItem(SD.StudentStatusPending, SD.StudentStatusPending);
        private static SelectListItem instructor = new SelectListItem(SD.StudentStatusAccepted, SD.StudentStatusAccepted);
        public static IEnumerable<SelectListItem> DocTypeList = new List<SelectListItem>() { all, instructor};

        public const string PublicSchool = "Public School";

        //Roles
        public const string Role_Admin = "Admin";
        public const string Role_Social = "Social Worker";
        public const string Role_Instructor = "Instructor";
        public const string Role_Rater = "Rater";
        public const string Role_User_Indi = "Individual";
    }
}
