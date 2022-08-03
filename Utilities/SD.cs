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

        // Student status
        public const string StudentStatusApplicant = "Applicant";
        public const string StudentStatusRejected = "Rejected";
        public const string StudentStatusAccepted = "Student";
        public const string StudentStatusPending = "Pending";
        public const string StudentStatusWaitlist = "Waitlisted";
        public const string StudentStatusWithdrawn = "Withdrawn";

        private static SelectListItem pending = new SelectListItem(StudentStatusPending, StudentStatusPending);
        private static SelectListItem accepted = new SelectListItem(StudentStatusAccepted, StudentStatusAccepted);
        private static SelectListItem rejected = new SelectListItem(StudentStatusRejected, StudentStatusRejected);
        private static SelectListItem waitlisted = new SelectListItem(StudentStatusWaitlist, StudentStatusWaitlist);
        private static SelectListItem withdrawn = new SelectListItem(StudentStatusWithdrawn, StudentStatusWithdrawn);
        public static IEnumerable<SelectListItem> StudentStatusList = new List<SelectListItem>() { pending, accepted, rejected, waitlisted, withdrawn };

        // Doc Types
        private static SelectListItem all = new SelectListItem(StudentStatusPending, StudentStatusPending);
        private static SelectListItem instructor = new SelectListItem(StudentStatusAccepted, StudentStatusAccepted);
        public static IEnumerable<SelectListItem> DocTypeList = new List<SelectListItem>() { all, instructor};

        // Schools
        public const string SchoolPublic = "Public School";
        public const string SchoolBoanne = "Boanne";

        // Guardian relationship types
        public const string GuardianRelationFather = "Father";
        public const string GuardianRelationMother = "Mother";
        public const string GuardianRelationUncle = "Uncle";
        public const string GuardianRelationAunt = "Aunt";
        public const string GuardianRelationSister = "Sister";
        public const string GuardianRelationBrother = "Brother";
        public const string GuardianRelationCousin = "Cousin";
        private static SelectListItem father = new SelectListItem(GuardianRelationFather, GuardianRelationFather);
        private static SelectListItem mother = new SelectListItem(GuardianRelationMother, GuardianRelationMother);
        private static SelectListItem uncle = new SelectListItem(GuardianRelationUncle, GuardianRelationUncle);
        private static SelectListItem aunt = new SelectListItem(GuardianRelationAunt, GuardianRelationAunt);
        private static SelectListItem sister = new SelectListItem(GuardianRelationSister, GuardianRelationSister);
        private static SelectListItem brother = new SelectListItem(GuardianRelationBrother, GuardianRelationBrother);
        private static SelectListItem cousin = new SelectListItem(GuardianRelationCousin, GuardianRelationCousin);
        public static IEnumerable<SelectListItem> GuardianRelationshipList = new List<SelectListItem>() { father, mother, uncle, aunt, sister, brother, cousin };

        //Roles
        public const string Role_Admin = "Admin";
        public const string Role_Social = "Social Worker";
        public const string Role_Instructor = "Instructor";
        public const string Role_Instructor_English = "English Instructor";
        public const string Role_Instructor_IT = "IT Instructor";
        public const string Role_Rater = "Rater";
        public const string Role_User_Indi = "Individual";

        //Priority items for Student Notes
        public const string PriorityLow = "Low";
        public const string PriorityMedium = "Medium";
        public const string PriorityHigh = "High";
        public const string PriorityComplete = "Complete";
        public static List<string> PriorityList = new List<string>() { PriorityLow, PriorityMedium, PriorityHigh, PriorityComplete };
    }
}
