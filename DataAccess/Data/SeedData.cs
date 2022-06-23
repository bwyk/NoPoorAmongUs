using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Academic;

namespace DataAccess.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Instructors.Any())
            {
                context.Instructors.AddRange(
                    new Instructor
                    {
                        Name = "Cindy Reed",
                        Email = "CindyR@gmail.com",
                        Role = "Social Worker"
                    },
                    new Instructor
                    {
                        Name = "Sam Price",
                        Email = "SamP@gmail.com",
                        Role = "Admin"
                    },
                    new Instructor
                    {
                        Name = "Alice Peterson",
                        Email = "AliceP@gmail.com",
                        Role = "Instructor"
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course
                    {
                        Name = "English 1"
                    },
                    new Course
                    {
                        Name = "Computers 1"
                    }
                );
                context.SaveChanges();
            }
            if (!context.Applicants.Any())
            {
                context.Applicants.AddRange(
                    new Applicant
                    {
                        Status = "Applicant",
                        Name = "Sara Blue",
                        Birthday = "January 5, 2006",
                        Village = "Village 1",
                        Address = "123 W",
                        Phone = "258 234 567"

                    },
                    new Applicant
                    {
                        Status = "Applicant",
                        Name = "Cindy Adams",
                        Birthday = "April 24, 2005",
                        Village = "Village 2",
                        Address = "789 S",
                        Phone = "258 123 456"
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Schools.Any())
            {
                context.Schools.AddRange(
                    new School
                    {
                        Name = "Public School"
                    },
                    new School
                    {
                        Name = "Boanne"
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Guardians.Any())
            {
                context.Guardians.AddRange(
                    new Guardian
                    {
                        Name = "Jessica Blue",
                        Relationship = "Mother",
                        ApplicantId = 3
                    },
                    new Guardian
                    {
                        Name = "Tom Adams",
                        Relationship = "Father",
                        ApplicantId = 4
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student
                    {
                        Name = "Sara Blue",
                        Birthday = "January 5, 2006",
                        Village = "Village 1",
                        Address = "123 W",
                        Phone = "258 234 567",
                        EnglishLevel = 3,
                        ComputerLevel = 4,
                        ApplicantId = 3
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Terms.Any())
            {
                context.Terms.AddRange(
                    new Term
                    {
                        Name = "Fall 2022",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(4),
                        IsActive = true

                    }
                    );
                context.SaveChanges();
            }
            if (!context.Classes.Any())
            {
                context.Classes.AddRange(
                    new Class
                    {
                        InstructorId = 11,
                        SchoolId = 4,
                        TermId = 1,
                        CourseId = 2
                    }
                    );
                context.SaveChanges();
            }
            if (!context.Ratings.Any())
            {
                context.Ratings.AddRange(
                    new Rating
                    {
                        Age = 16,
                        SchoolLevel = 10,
                        Academics = 3,
                        FoodAssistance = 1,
                        AnnualIncome = 200,
                        Determination = 5,
                        FamilySupport = 1,
                        ApplicantId = 3
                    }
                    );
                context.SaveChanges();
            }
        }
    }

}
