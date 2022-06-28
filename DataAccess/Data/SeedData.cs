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
                    new Subject
                    {
                        Name = "English 1"
                    },
                    new Subject
                    {
                        Name = "Computers 1"
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
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student
                    {
                        Status = "Student",
                        FirstName = "Sara",
                        LastName = "Blue",
                        Birthday = new DateTime(2005, 12, 25),
                        Village = "Village 1",
                        Address = "123 W",
                        Phone = "258 234 567",
                        EnglishLevel = 3,
                        ComputerLevel = 4
                    },
                    new Student
                    {
                        Status = "Applicant",
                        FirstName = "Cindy",
                        LastName = "Adams",
                        Birthday = new DateTime(2006, 1, 1),
                        Village = "Village 2",
                        Address = "789 S",
                        Phone = "258 123 456",
                        EnglishLevel = 1,
                        ComputerLevel = 2
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
                        StudentId = 1
                    },
                    new Guardian
                    {
                        Name = "Tom Adams",
                        Relationship = "Father",
                        StudentId = 2
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
                    new Course
                    {
                        InstructorId = 1,
                        SchoolId = 2,
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
                        StudentId = 2
                    }
                    );
                context.SaveChanges();
            }
        }
    }

}
