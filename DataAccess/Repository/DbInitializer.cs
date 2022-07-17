using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Academic;
using Utilities;

namespace DataAccess.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        private string english1CourseName = "Course: English 1";
        private string computers1CourseName = "Course: Computers 1";


        public DbInitializer(
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
            SeedGuardians();
            //SeedCourses();
            SeedCourseSession();
            SeedCourseEnrollment();

            //Create roles
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Social)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Rater)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();

            //Create "Super Admins"
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "kevinmclennan@mail.weber.edu",
                Email = "kevinmclennan@mail.weber.edu",
                FirstName = "Kevin",
                LastName = "McLennan"
            }, "Test12345!").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u => u.Email == "kevinmclennan@mail.weber.edu");

            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }

        private (Student, Student) GetStudents()
        {
            bool saveChanges = false;
            Student? saraBlue = _db.Students.FirstOrDefault(s => s.LastName == "Blue");
            Student? cindyAdams = _db.Students.FirstOrDefault(s => s.LastName == "Adams");
            if (saraBlue is null)
            {
                _db.Students.Add(
                    saraBlue = new Student
                    {
                        Status = SD.StudentStatusAccepted,
                        FirstName = "Sara",
                        LastName = "Blue",
                        Birthday = new DateTime(2006, 01, 05),
                        Village = "Village 1",
                        Address = "123 W",
                        Phone = "258 234 567",
                        EnglishLevel = 3,
                        ComputerLevel = 4,
                    }
                );
                saveChanges = true;
            }
            if (cindyAdams is null)
            {
                _db.Students.Add(
                    cindyAdams = new Student
                    {
                        Status = SD.StudentStatusPending,
                        FirstName = "Cindy",
                        LastName = "Adams",
                        Birthday = new DateTime(2005, 04, 24),
                        Village = "Village 2",
                        Address = "789 S",
                        Phone = "258 123 456"
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
            return (saraBlue, cindyAdams);
        }

        private void SeedGuardians()
        {
            bool saveChanges = false;
            Student saraBlue;
            Student cindyAdams;
            (saraBlue, cindyAdams) = GetStudents();
            SeedRatings(cindyAdams);
            Guardian? saraBlueGuardian = _db.Guardians.FirstOrDefault(g => g.LastName == "Blue");
            Guardian? cindyAdamsGuardian = _db.Guardians.FirstOrDefault(g => g.LastName == "Adams");
            if (saraBlueGuardian is null)
            {
                _db.Guardians.Add(
                    new Guardian
                    {
                        FirstName = "Jessica",
                        LastName = "Blue",
                        Relationship = "Mother",
                        StudentId = saraBlue.Id
                    }
                );                
                saveChanges = true;
            }
            if (cindyAdamsGuardian is null)
            {
                _db.Guardians.Add(
                    new Guardian
                    {
                        FirstName = "Tom",
                        LastName = "Adams",
                        Relationship = "Father",
                        StudentId = cindyAdams.Id
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
        }

        private void SeedRatings(Student student)
        {
            //TODO make it randomly pick from list of ratings

            //TODO filter based on application status? or leave as future students will still have ratings
            //if (student.Status != Role.Status_Pending)

            //    bool saveChanges = false;
            //if (!_db.Ratings.Any(r => r.StudentId == student.Id))
            //{
            //    _db.Ratings.AddRange(
            //        new Rating
            //        {
            //            Age = 16,
            //            SchoolLevel = 10,
            //            Academics = 3,
            //            FoodAssistance = 1,
            //            AnnualIncome = 200,
            //            Determination = 5,
            //            FamilySupport = 1,
            //            StudentId = student.Id
            //        }
            //    );
            //    saveChanges = true;
            //}
            //if (saveChanges)
            //    _db.SaveChanges();
        }
       
        private (Course, Course) GetCourses()
        {
            bool saveChanges = false;
            Instructor cindyReed;
            Instructor samPrice;
            Instructor alicePeterson;
            (cindyReed, samPrice, alicePeterson) = GetInstructors();
            School publicSchool;
            School boanne;
            (publicSchool, boanne) = GetSchools();
            Term fall22 = GetTerm();
            Subject english;
            Subject computers;
            (english, computers) = GetSubjects();
            Course? computers1 = _db.Courses.FirstOrDefault(c => c.Name == english1CourseName);
            Course? english1 = _db.Courses.FirstOrDefault(c => c.Name == computers1CourseName);
            if (computers1 is null)
            {
                _db.Courses.Add(
                    computers1 = new Course
                    {
                        Name = computers1CourseName,
                        InstructorId = cindyReed.Id,
                        TermId = fall22.Id,
                        SchoolId = boanne.Id,
                        SubjectId = computers.Id
                    }
                );
                saveChanges = true;
            }
            if (english1 is null)
            {
                _db.Courses.Add(
                    english1 = new Course
                    {
                        Name = english1CourseName,
                        InstructorId = alicePeterson.Id,
                        SchoolId = publicSchool.Id,
                        TermId = fall22.Id,
                        SubjectId = english.Id
                    }
                );
                saveChanges = true;
            }
            if(saveChanges)
                _db.SaveChanges();

            return (english1, computers1);
        }
        private (CourseSession, CourseSession) GetCourseSessions()
        {
            bool saveChanges = false;
            Course courseEnglish1;
            Course courseComputers1;
            (courseEnglish1, courseComputers1) = GetCourses();
            CourseSession? sessionEnglish1 = _db.CourseSessions.FirstOrDefault(g => g.Course == courseEnglish1);
            CourseSession? sessionComputers1 = _db.CourseSessions.FirstOrDefault(g => g.Course == courseComputers1);
            if (sessionEnglish1 is null)
            {
                _db.CourseSessions.Add(
                    sessionEnglish1 = new CourseSession
                    {
                        CourseId = courseEnglish1.Id,
                        Course = courseEnglish1,
                        CourseName = courseEnglish1.Name,
                        Day = "Monday"
                    }
                );
                saveChanges = true;
            }
            if (sessionComputers1 is null)
            {
                _db.CourseSessions.Add(
                    sessionComputers1 = new CourseSession
                    {
                        CourseId = courseComputers1.Id,
                        Course = courseComputers1,
                        CourseName = courseComputers1.Name,
                        Day = "Tuesday"
                    }
                );
                saveChanges = true;
            }
            if(saveChanges)
                _db.SaveChanges();

            return (sessionEnglish1, sessionComputers1);
        }
        private void SeedCourseEnrollment()
        {
            bool saveChanges = false;
            Student saraBlue;
            Student cindyAdams;
            (saraBlue, cindyAdams) = GetStudents();
            CourseSession sessionEnglish1;
            CourseSession sessionComputers1;
            (sessionEnglish1, sessionComputers1) = GetCourseSessions();
            CourseEnrollment? saraBlueEnrollment = _db.CourseEnrollments.FirstOrDefault(
                    e => ((e.CourseSessionId == sessionEnglish1.Id) && (e.StudentId == saraBlue.Id)));
            CourseEnrollment? cindyAdamEnrollment = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionComputers1.Id) && (e.StudentId == cindyAdams.Id)));
            if (saraBlueEnrollment is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionEnglish1.CourseId,
                        StudentId = saraBlue.Id,
                        CourseSession = sessionEnglish1,
                        Student = saraBlue

                    }
                );
                saveChanges = true;
            }
            if (cindyAdamEnrollment is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionComputers1.CourseId,
                        StudentId = cindyAdams.Id,
                        CourseSession = sessionComputers1,
                        Student = cindyAdams
                    }
                );
                saveChanges = true;
            }
            if(saveChanges)
                _db.SaveChanges();
        }
        
       
        private void SeedCourseSession()
        {
            bool saveChanges = false;
            Course courseEnglish1;
            Course courseComputers1;
            (courseEnglish1, courseComputers1) = GetCourses();
            Subject subjectEnglish1;
            Subject subjectComputers1;
            (subjectEnglish1, subjectComputers1) = GetSubjects();
            CourseSession? sessionEnglish1 = _db.CourseSessions.FirstOrDefault(s => s.CourseName == english1CourseName);
            CourseSession? sessionComputers1 = _db.CourseSessions.FirstOrDefault(s => s.CourseName == computers1CourseName);
            if (sessionComputers1 is null)
            {
                _db.Add(
                    new CourseSession
                    {
                        CourseId = courseComputers1.Id,
                        Course = courseComputers1,
                        CourseName = computers1CourseName,
                        Day = "Monday"
                    }
                );
                saveChanges = true;
            }
            if (sessionEnglish1 is null)
            {
                _db.Add(
                    new CourseSession
                    {
                        CourseId = courseEnglish1.Id,
                        Course = courseEnglish1,
                        CourseName = english1CourseName,
                        Day = "Tuesday"
                    }
                );
                saveChanges = true;
            }
            if(saveChanges)
                _db.SaveChanges();
        }
        
        private Term GetTerm()
        {
            bool saveChanges = false;
            Term? fall2022 = _db.Terms.FirstOrDefault(g => g.Name == "Fall 2022");
            if (fall2022 is null)
            {
                _db.Terms.Add(
                    fall2022 = new Term
                    {
                        Name = "Fall 2022",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(4),
                        IsActive = true
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return fall2022;
        }

        private (Instructor, Instructor, Instructor) GetInstructors()
        {
            bool saveChanges = false;
            Instructor? cindyReed = _db.Instructors.FirstOrDefault(i => i.LastName == "Reed");
            Instructor? samPrice = _db.Instructors.FirstOrDefault(i => i.LastName == "Price");
            Instructor? alicePeterson = _db.Instructors.FirstOrDefault(i => i.LastName == "Peterson");

            if (cindyReed is null)
            {
                _db.Add(
                    cindyReed = new Instructor
                    {
                        FirstName = "Cindy",
                        LastName = "Reed",
                        Email = "CindyR@gmail.com",
                        Role = "Social Worker"
                    }
                );
                saveChanges = true;
            }
            if (samPrice is null)
            {
                _db.Add(
                    samPrice = new Instructor
                    {
                        FirstName = "Sam",
                        LastName = "Price",
                        Email = "SamP@gmail.com",
                        Role = "Admin"
                    }
                );
                saveChanges = true;
            }
            if (alicePeterson is null)
            {
                _db.Add(
                    alicePeterson = new Instructor
                    {
                        FirstName = "Alice",
                        LastName = "Peterson",
                        Email = "AliceP@gmail.com",
                        Role = "Instructor"
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
            return (cindyReed, samPrice, alicePeterson);
        }

        private (Subject, Subject) GetSubjects()
        {
            bool saveChanges = false;
            Subject? subjectEnglish1 = _db.Subjects.FirstOrDefault(s => s.Name == "English 1");
            Subject? subjectComputers1 = _db.Subjects.FirstOrDefault(s => s.Name == "Computers 1");
            if (subjectEnglish1 is null)
            {
                _db.Add(
                   subjectEnglish1 = new Subject
                    {
                        Name = "English 1"
                    }
                );
                saveChanges = true;
            }
            if (subjectComputers1 is null)
            {
                _db.Add(
                    subjectComputers1 = new Subject
                    {
                        Name = "Computers 1"
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return (subjectEnglish1, subjectComputers1);
        }

        private (School, School) GetSchools()
        {
            bool saveChanges = false;
            School? publicSchool = _db.Schools.FirstOrDefault(s => s.Name == "Public School");
            School? boanne = _db.Schools.FirstOrDefault(s => s.Name == "Boanne");
            if (publicSchool is null)
            {
                _db.Add(
                    publicSchool = new School
                    {
                        Name = "Public School"
                    }
                );
                saveChanges = true;
            }
            if (boanne is null)
            {
                _db.Add(
                    boanne = new School
                    {
                        Name = "Boanne"
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return (publicSchool, boanne);
        }

    }
}

