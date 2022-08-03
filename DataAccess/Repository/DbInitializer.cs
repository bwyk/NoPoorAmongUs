using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Academic;
using Models.People;
using Utilities;

namespace DataAccess.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        private string publicCourseName = "Course: Public";
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
            //Create roles
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Social)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Rater)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor_English)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Instructor_IT)).GetAwaiter().GetResult();

            //SeedGuardians();
            //SeedCourses();
            SeedRelationships();
            SeedCourseEnrollment();
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

            //SeedGuardians();
            //SeedCourses();
            SeedRelationships();
            SeedCourseSessionAsync();
            SeedCourseEnrollmentAsync();
            SeedNoteTypes().GetAwaiter().GetResult();
            SeedStudentNotes();
        }

        private (Student, Student, Student, Student, Student) GetStudents()
        {
            bool saveChanges = false;
            Student? m_Boa = _db.Students.FirstOrDefault(s => s.LastName == "Boaventura");
            Student? a_Arl = _db.Students.FirstOrDefault(s => s.LastName == "Arlindo");
            Student? r_Ale = _db.Students.FirstOrDefault(s => s.LastName == "Alegria");
            Student? a_Mac = _db.Students.FirstOrDefault(s => s.LastName == "Machava");
            Student? a_Fra = _db.Students.FirstOrDefault(s => s.LastName == "Francisco");
            if (m_Boa is null)
            {
                _db.Students.Add(
                    m_Boa = new Student
                    {
                        Status = SD.StudentStatusAccepted,
                        FirstName = "Marlene",
                        LastName = "Boaventura",
                        Birthday = new DateTime(2008, 01, 05),
                        Village = "Boane",
                        Address = "Unknown",
                        EnglishLevel = 1,
                        ComputerLevel = 1,
                    }
                );
                saveChanges = true;
            }
            if (a_Arl is null)
            {
                _db.Students.Add(
                    a_Arl = new Student
                    {
                        Status = SD.StudentStatusPending,
                        FirstName = "Anstancia",
                        LastName = "Arlindo",
                        Birthday = new DateTime(2009, 04, 24),
                        Village = "Boane",
                        Address = "Unknown",
                        EnglishLevel = 0,
                        ComputerLevel = 0,
                    }
                );
                saveChanges = true;
            }
            if (r_Ale is null)
            {
                _db.Students.Add(
                    r_Ale = new Student
                    {
                        Status = SD.StudentStatusAccepted,
                        FirstName = "Rita",
                        LastName = "Alegria",
                        Birthday = new DateTime(2007, 04, 24),
                        Village = "Boane",
                        Address = "Unknown",
                        EnglishLevel = 1,
                        ComputerLevel = 1,
                    }
                );
                saveChanges = true;
            }
            if (a_Mac is null)
            {
                _db.Students.Add(
                    a_Mac = new Student
                    {
                        Status = SD.StudentStatusAccepted,
                        FirstName = "Albertina",
                        MiddleName = "Andre",
                        LastName = "Machava",
                        Birthday = new DateTime(2006, 04, 24),
                        Village = "Boane",
                        Address = "Unknown",
                        EnglishLevel = 1,
                        ComputerLevel = 1,
                    }
                );
                saveChanges = true;
            }
            if (a_Fra is null)
            {
                _db.Students.Add(
                    a_Fra = new Student
                    {
                        Status = SD.StudentStatusPending,
                        FirstName = "Antonieta",
                        MiddleName = "Vitorino",
                        LastName = "Francisco",
                        Birthday = new DateTime(2006, 04, 24),
                        Village = "Boane",
                        Address = "Unknown",
                        EnglishLevel = 1,
                        ComputerLevel = 1,
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
            return (m_Boa, a_Arl, r_Ale, a_Mac, a_Fra);
        }

        private void SeedRelationships()
        {
            bool saveChanges = false;

            Student m_Boa;
            Student a_Arl;
            Student r_Ale;
            Student a_Mac;
            Student a_Fra;
            (m_Boa, a_Arl, r_Ale, a_Mac, a_Fra) = GetStudents();

            Guardian mama_Boa;
            Guardian papa_Arl;
            Guardian uncle_Ale;
            Guardian cousin_Mac;
            Guardian sister_Fra;
            (mama_Boa, papa_Arl, uncle_Ale, cousin_Mac, sister_Fra) = GetGuardians();
            var relationships = _db.Relationships.ToList();
            if (!relationships.Any(r => (r.GuardianId == mama_Boa.Id) && (r.StudentId == m_Boa.Id)))
            {
                _db.Relationships.Add(new Relationship()
                {
                    Guardian = mama_Boa,
                    GuardianId = mama_Boa.Id,
                    Student = m_Boa,
                    StudentId = m_Boa.Id,
                    RelationshipType = SD.GuardianRelationMother

                });
                saveChanges = true;
            }
            if (!relationships.Any(r => (r.GuardianId == papa_Arl.Id) && (r.StudentId == a_Arl.Id)))
            {
                _db.Relationships.Add(new Relationship()
                {
                    Guardian = papa_Arl,
                    GuardianId = papa_Arl.Id,
                    Student = a_Arl,
                    StudentId = a_Arl.Id,
                    RelationshipType = SD.GuardianRelationFather
                });
                saveChanges = true;

            }
            if (!relationships.Any(r => (r.GuardianId == uncle_Ale.Id) && (r.StudentId == r_Ale.Id)))
            {
                _db.Relationships.Add(new Relationship()
                {
                    Guardian = uncle_Ale,
                    GuardianId = uncle_Ale.Id,
                    Student = r_Ale,
                    StudentId = r_Ale.Id,
                    RelationshipType = SD.GuardianRelationUncle
                });
                saveChanges = true;

            }
            if (!relationships.Any(r => (r.GuardianId == cousin_Mac.Id) && (r.StudentId == a_Mac.Id)))
            {
                _db.Relationships.Add(new Relationship()
                {
                    Guardian = cousin_Mac,
                    GuardianId = cousin_Mac.Id,
                    Student = a_Mac,
                    StudentId = a_Mac.Id,
                    RelationshipType = SD.GuardianRelationCousin
                });
                saveChanges = true;

            }
            if (!relationships.Any(r => (r.GuardianId == sister_Fra.Id) && (r.StudentId == a_Fra.Id)))
            {
                _db.Relationships.Add(new Relationship()
                {
                    Guardian = sister_Fra,
                    GuardianId = sister_Fra.Id,
                    Student = a_Fra,
                    StudentId = a_Fra.Id,
                    RelationshipType = SD.GuardianRelationSister
                });
                saveChanges = true;

            }
            if (saveChanges)
                _db.SaveChanges();
        }
        private (Guardian, Guardian, Guardian, Guardian, Guardian) GetGuardians()
        {
            bool saveChanges = false;
            Student m_Boa;
            Student a_Arl;
            Student r_Ale;
            Student a_Mac;
            Student a_Fra;
            (m_Boa, a_Arl, r_Ale, a_Mac, a_Fra) = GetStudents();
            //SeedRatings(cindyAdams);
            Guardian? mama_Boa = _db.Guardians.FirstOrDefault(g => g.LastName == "Boaventura");
            Guardian? papa_Arl = _db.Guardians.FirstOrDefault(g => g.LastName == "Arlindo");
            Guardian? uncle_Ale = _db.Guardians.FirstOrDefault(g => g.LastName == "Alegria");
            Guardian? cousin_Mac = _db.Guardians.FirstOrDefault(g => g.LastName == "Machava");
            Guardian? sister_Fra = _db.Guardians.FirstOrDefault(g => g.LastName == "Francisco");

            if (mama_Boa is null)
            {
                _db.Guardians.Add(
                    mama_Boa = new Guardian
                    {
                        FirstName = "Marla",
                        LastName = "Boaventura"
                    }
                );
                saveChanges = true;
            }
            if (papa_Arl is null)
            {
                _db.Guardians.Add(
                    papa_Arl = new Guardian
                    {
                        FirstName = "Cesar",
                        LastName = "Arlindo"
                    }
                );
                saveChanges = true;
            }
            if (uncle_Ale is null)
            {
                _db.Guardians.Add(
                    uncle_Ale = new Guardian
                    {
                        FirstName = "Albert",
                        LastName = "Alegria"
                    }
                );
                saveChanges = true;
            }
            if (cousin_Mac is null)
            {
                _db.Guardians.Add(
                    cousin_Mac = new Guardian
                    {
                        FirstName = "Antonio",
                        MiddleName = "Andre",
                        LastName = "Machava"
                    }
                );
                saveChanges = true;
            }
            if (sister_Fra is null)
            {
                _db.Guardians.Add(
                    sister_Fra = new Guardian
                    {
                        FirstName = "Amelia",
                        MiddleName = "Nossa",
                        LastName = "Francisco"
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
            return (mama_Boa, papa_Arl, uncle_Ale, cousin_Mac, sister_Fra);
        }

        private void SeedRatings(Student student)
        {
            //TODO make it randomly pick from list of ratings
            //TODO filter based on application status? or leave as future students will still have ratings
            //if(student.Status != Role.Status_Pending)
            bool saveChanges = false;
            if (!_db.Ratings.Any(r => r.StudentId == student.Id))
            {
                _db.Ratings.AddRange(
                    new Rating
                    {
                        Age = 16,
                        Academics = 3,
                        AnnualIncome = 200,
                        FamilySupport = 1,
                        Distance = 5,
                        StudentId = student.Id
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
        }

        private (Course, Course, Course) GetCourses()
        {
            bool saveChanges = false;
            ApplicationUser publicInstructor;
            ApplicationUser j_Phillips;
            ApplicationUser a_Nelson;
            ApplicationUser e_Mussane;
            ApplicationUser celeste;
            (publicInstructor, j_Phillips, a_Nelson, e_Mussane, celeste) = GetInstructors();

            School publicSchool;
            School boanne;
            (publicSchool, boanne) = GetSchools();
            Term fall22 = GetTerm();
            Subject publicSubject;
            Subject english;
            Subject computers;
            (publicSubject, english, computers) = GetSubjects();
            Course? publicCourse = _db.Courses.FirstOrDefault(c => c.Name == publicCourseName);
            Course? computers1 = _db.Courses.FirstOrDefault(c => c.Name == computers1CourseName);
            Course? english1 = _db.Courses.FirstOrDefault(c => c.Name == english1CourseName);
            if (publicCourse is null)
            {
                _db.Courses.Add(
                    publicCourse = new Course
                    {
                        Name = publicCourseName,
                        InstructorId = publicInstructor.Id,
                        TermId = fall22.Id,
                        SchoolId = publicSchool.Id,
                        SubjectId = publicSubject.Id
                    }
                );
            }
            if (computers1 is null)
            {
                _db.Courses.Add(
                    computers1 = new Course
                    {
                        Name = computers1CourseName,
                        InstructorId = e_Mussane.Id,
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
                        InstructorId = a_Nelson.Id,
                        SchoolId = boanne.Id,
                        TermId = fall22.Id,
                        SubjectId = english.Id
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return (publicCourse, english1, computers1);
        }

        private (CourseSession, CourseSession, CourseSession) GetCourseSessions()
        {
            bool saveChanges = false;
            Course coursePublic;
            Course courseEnglish1;
            Course courseComputers1;
            (coursePublic, courseEnglish1, courseComputers1) = GetCourses();
            CourseSession? sessionPublic = _db.CourseSessions.FirstOrDefault(g => g.Course == coursePublic);
            CourseSession? sessionEnglish1 = _db.CourseSessions.FirstOrDefault(g => g.Course == courseEnglish1);
            CourseSession? sessionComputers1 = _db.CourseSessions.FirstOrDefault(g => g.Course == courseComputers1);
            if (sessionPublic is null)
            {
                _db.CourseSessions.Add(
                    sessionPublic= new CourseSession
                    {
                        CourseId = coursePublic.Id,
                        Course = coursePublic,
                        CourseName = coursePublic.Name,
                        Day = SD.Weekdays.Thursday.ToString(),
                        EndTime = new DateTime(2022, 08, 02, 10, 30, 00),
                        StartTime = new DateTime(2022, 08, 02, 11, 30, 00),
                    }
                );
                saveChanges = true;
            }
            if (sessionEnglish1 is null)
            {
                _db.CourseSessions.Add(
                    sessionEnglish1 = new CourseSession
                    {
                        CourseId = courseEnglish1.Id,
                        Course = courseEnglish1,
                        CourseName = courseEnglish1.Name,
                        Day = SD.Weekdays.Monday.ToString(),
                        EndTime = new DateTime(2022, 08, 02, 12, 30, 00),
                        StartTime = new DateTime(2022, 08, 02, 14, 30, 00),
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
                        Day = SD.Weekdays.Tuesday.ToString(),
                        EndTime = new DateTime(2022, 08, 02, 15, 00, 00),
                        StartTime = new DateTime(2022, 08, 02, 17, 30, 00),

                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return (sessionPublic, sessionEnglish1, sessionComputers1);
        }

        private void SeedCourseEnrollment()
        {
            bool saveChanges = false;
            Student m_Boa;
            Student a_Arl;
            Student r_Ale;
            Student a_Mac;
            Student a_Fra;
            (m_Boa, a_Arl, r_Ale, a_Mac, a_Fra) = GetStudents();
            CourseSession sessionPublic;
            CourseSession sessionEnglish1;
            CourseSession sessionComputers1;
            (sessionPublic, sessionEnglish1, sessionComputers1) = GetCourseSessions();

            CourseEnrollment? m_Boa_Enrollment_Eng = _db.CourseEnrollments.FirstOrDefault(
                    e => ((e.CourseSessionId == sessionEnglish1.Id) && (e.StudentId == m_Boa.Id)));
            CourseEnrollment? m_Boa_Enrollment_Com = _db.CourseEnrollments.FirstOrDefault(
                    e => ((e.CourseSessionId == sessionComputers1.Id) && (e.StudentId == m_Boa.Id)));
            CourseEnrollment? m_Boa_Enrollment_Pub = _db.CourseEnrollments.FirstOrDefault(
                    e => ((e.CourseSessionId == sessionPublic.Id) && (e.StudentId == m_Boa.Id)));

            CourseEnrollment? r_Ale_Enrollment_Eng = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionEnglish1.Id) && (e.StudentId == r_Ale.Id)));
            CourseEnrollment? r_Ale_Enrollment_Com = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionComputers1.Id) && (e.StudentId == r_Ale.Id)));
            CourseEnrollment? r_Ale_Enrollment_Pub = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionPublic.Id) && (e.StudentId == r_Ale.Id)));

            CourseEnrollment? a_Mac_Enrollment_Eng = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionEnglish1.Id) && (e.StudentId == a_Mac.Id)));
            CourseEnrollment? a_Mac_Enrollment_Com = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionComputers1.Id) && (e.StudentId == a_Mac.Id)));
            CourseEnrollment? a_Mac_Enrollment_Pub = _db.CourseEnrollments.FirstOrDefault(
                e => ((e.CourseSessionId == sessionPublic.Id) && (e.StudentId == a_Mac.Id)));
            if (m_Boa_Enrollment_Eng is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionEnglish1.CourseId,
                        StudentId = m_Boa.Id,
                        CourseSession = sessionEnglish1,
                        Student = m_Boa
                    }
                );
                saveChanges = true;
            }
            if (m_Boa_Enrollment_Com is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionComputers1.CourseId,
                        StudentId = m_Boa.Id,
                        CourseSession = sessionComputers1,
                        Student = m_Boa
                    }
                );
                saveChanges = true;
            }
            if (m_Boa_Enrollment_Pub is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionPublic.CourseId,
                        StudentId = m_Boa.Id,
                        CourseSession = sessionPublic,
                        Student = m_Boa
                    }
                );
                saveChanges = true;
            }

            if (r_Ale_Enrollment_Eng is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionEnglish1.CourseId,
                        StudentId = r_Ale.Id,
                        CourseSession = sessionEnglish1,
                        Student = r_Ale
                    }
                );
                saveChanges = true;
            }
            if (r_Ale_Enrollment_Com is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionComputers1.CourseId,
                        StudentId = r_Ale.Id,
                        CourseSession = sessionComputers1,
                        Student = r_Ale
                    }
                );
                saveChanges = true;
            }
            if (r_Ale_Enrollment_Pub is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionPublic.CourseId,
                        StudentId = r_Ale.Id,
                        CourseSession = sessionPublic,
                        Student = r_Ale
                    }
                );
                saveChanges = true;
            }

            if (a_Mac_Enrollment_Eng is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionEnglish1.CourseId,
                        StudentId = a_Mac.Id,
                        CourseSession = sessionEnglish1,
                        Student = a_Mac
                    }
                );
                saveChanges = true;
            }
            if (a_Mac_Enrollment_Com is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionComputers1.CourseId,
                        StudentId = a_Mac.Id,
                        CourseSession = sessionComputers1,
                        Student = a_Mac
                    }
                );
                saveChanges = true;
            }
            if (a_Mac_Enrollment_Pub is null)
            {
                _db.CourseEnrollments.Add(
                    new CourseEnrollment
                    {
                        CourseSessionId = sessionPublic.CourseId,
                        StudentId = a_Mac.Id,
                        CourseSession = sessionPublic,
                        Student = a_Mac
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
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

        private (ApplicationUser, ApplicationUser, ApplicationUser, ApplicationUser, ApplicationUser) GetInstructors()
        {
            bool saveChanges = false;
            ApplicationUser? publicInstructor =_db.ApplicationUser.FirstOrDefault(i => i.FirstName == "Public");
            ApplicationUser? j_Phillips =_db.ApplicationUser.FirstOrDefault(i => i.LastName == "Phillips");
            ApplicationUser? a_Nelson =  _db.ApplicationUser.FirstOrDefault(i => i.LastName == "Nelson");
            ApplicationUser? e_Mussane = _db.ApplicationUser.FirstOrDefault(i => i.LastName == "Mussane");
            ApplicationUser? celeste =   _db.ApplicationUser.FirstOrDefault(i => i.FirstName == "Celeste");
            if (publicInstructor is null)
            {
                _db.Add(
                    publicInstructor = new ApplicationUser
                    {
                        FirstName = "Public",
                        LastName = "Instructor",
                        Email = "placeholder@gmail.com"
                    }
                );
                _userManager.AddToRoleAsync(publicInstructor, SD.Role_Instructor).GetAwaiter().GetResult();
                saveChanges = true;
            }
            if (j_Phillips is null)
            {
                _db.Add(
                    j_Phillips = new ApplicationUser
                    {
                        FirstName = "Josh",
                        LastName = "Phillips",
                        Email = "joshphllps14@gmail.com"
                    }
                );
                _userManager.AddToRoleAsync(j_Phillips, SD.Role_Admin).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(j_Phillips, SD.Role_Rater).GetAwaiter().GetResult();

                saveChanges = true;
            }
            if (a_Nelson is null)
            {
                _db.Add(
                    a_Nelson = new ApplicationUser
                    {
                        FirstName = "Agnaldo",
                        LastName = "Nelson",
                        Email = "agnaldodejesus4@gmail.com"
                    }
                );
                _userManager.AddToRoleAsync(a_Nelson, SD.Role_Instructor).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(a_Nelson, SD.Role_Rater).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(a_Nelson, SD.Role_Instructor_English).GetAwaiter().GetResult();
                saveChanges = true;
            }
            if (e_Mussane is null)
            {
                _db.Add(
                    e_Mussane = new ApplicationUser
                    {
                        FirstName = "Enfraime",
                        LastName = "Mussane",
                        Email = "novela1992@gmail.com"
                    }
                );
                _userManager.AddToRoleAsync(e_Mussane, SD.Role_Instructor).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(e_Mussane, SD.Role_Rater).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(e_Mussane, SD.Role_Instructor_IT).GetAwaiter().GetResult();
                saveChanges = true;
            }
            if (celeste is null)
            {
                _db.Add(
                    celeste = new ApplicationUser
                    {
                        FirstName = "Celeste",
                        LastName = "Unknown",
                        Email = "Uknown"
                    }
                );
                _userManager.AddToRoleAsync(celeste, SD.Role_Social).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(celeste, SD.Role_Rater).GetAwaiter().GetResult();

                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();
            return (publicInstructor, j_Phillips, a_Nelson, e_Mussane, celeste);
        }

        private (Subject, Subject, Subject) GetSubjects()
        {
            bool saveChanges = false;
            Subject? subjectPublic = _db.Subjects.FirstOrDefault(s => s.Name == "Public");
            Subject? subjectEnglish1 = _db.Subjects.FirstOrDefault(s => s.Name == "English 1");
            Subject? subjectComputers1 = _db.Subjects.FirstOrDefault(s => s.Name == "Computers 1");

            if (subjectPublic is null)
            {
                _db.Add(
                   subjectPublic = new Subject
                   {
                       Name = "Public"
                   }
                );
                saveChanges = true;
            }
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

            return (subjectPublic, subjectEnglish1, subjectComputers1);
        }

        private (School, School) GetSchools()
        {
            bool saveChanges = false;
            School? publicSchool = _db.Schools.FirstOrDefault(s => s.Name == SD.SchoolPublic);
            School? boanne = _db.Schools.FirstOrDefault(s => s.Name == SD.SchoolPublic);
            if (publicSchool is null)
            {
                _db.Add(
                    publicSchool = new School
                    {
                        Name = SD.SchoolPublic
                    }
                );
                saveChanges = true;
            }
            if (boanne is null)
            {
                _db.Add(
                    boanne = new School
                    {
                        Name = SD.SchoolBoanne
                    }
                );
                saveChanges = true;
            }
            if (saveChanges)
                _db.SaveChanges();

            return (publicSchool, boanne);
        }

        private async Task SeedNoteTypes()
        {
            IdentityRole Admin = await _roleManager.FindByNameAsync(SD.Role_Admin);
            IdentityRole SocialWorker = await _roleManager.FindByNameAsync(SD.Role_Social);
            IdentityRole Rater = await _roleManager.FindByNameAsync(SD.Role_Rater);
            IdentityRole Individual = await _roleManager.FindByNameAsync(SD.Role_User_Indi);
            IdentityRole Instructor = await _roleManager.FindByNameAsync(SD.Role_Instructor);
            string AdminId = await _roleManager.GetRoleIdAsync(Admin);
            string SocialWorkerId = await _roleManager.GetRoleIdAsync(SocialWorker);
            string RaterId = await _roleManager.GetRoleIdAsync(Rater);
            string IndividualId = await _roleManager.GetRoleIdAsync(Individual);
            string InstructorId = await _roleManager.GetRoleIdAsync(Instructor);

            var NoteType = new List<NoteType>
            {
                new NoteType{  Type = "Admin Note", RoleId = AdminId},
                new NoteType{  Type = "General Note", RoleId = AdminId},
                new NoteType{  Type = "General Note", RoleId = SocialWorkerId},
                new NoteType{  Type = "General Note", RoleId = RaterId},
                new NoteType{  Type = "General Note", RoleId = IndividualId},
                new NoteType{  Type = "General Note", RoleId = InstructorId}
            };

            foreach( var n in NoteType )
            {
                if(_db.NoteTypes.FirstOrDefault(nt => nt.Type == n.Type && nt.RoleId == n.RoleId) == null)
                {
                    _db.NoteTypes.Add(n);
                }
            }

            _db.SaveChanges();
        }
        private void SeedStudentNotes()
        {
            ApplicationUser user = _db.ApplicationUser.FirstOrDefault(u => u.Email == "kevinmclennan@mail.weber.edu");
            DateTime seedDate1 = new DateTime(1970, 1, 1);
            DateTime seedDate2 = new DateTime(1970, 1, 2);
            /*ApplicationUser AppUser = _db.ApplicationUser.Get(u => u.Id == userID);*/

            var StudentNote = new List<StudentNote>
            {
                new StudentNote{
                    Text = "<p>This is an admin note seeded into the database which can only be seen by users with the Admin Role.<p>",
                    CreatedDate = seedDate1,
                    Priority = SD.PriorityLow,
                    StudentId = 1,
                    NoteTypeId = 1,
                    ApplicationUser = user},
                new StudentNote{
                    Text = "<p>This is a general note seeded into the database which can be see by multiple roles setup through Note Type under Admin.<p>",
                    CreatedDate = seedDate2,
                    Priority = SD.PriorityComplete,
                    StudentId = 2,
                    NoteTypeId = 2,
                    ApplicationUser = user}
            };

            foreach (var sn in StudentNote)
            {
                if (_db.StudentNotes.FirstOrDefault(sn => sn.CreatedDate == seedDate1) == null || 
                    _db.StudentNotes.FirstOrDefault(sn => sn.CreatedDate == seedDate2) == null)
                {
                    _db.StudentNotes.Add(sn);
                }
            }

            _db.SaveChanges();
        }
    }
}