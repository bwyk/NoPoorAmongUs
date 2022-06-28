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
						SeedCourses();
				}

				private (Student, Student) GetStudents()
				{
						bool saveChanges = false;
						Student? saraBlue = _db.Students.FirstOrDefault(s => s.LastName == "Blue");
						Student? cindyAdams = _db.Students.FirstOrDefault(s => s.LastName == "Adams");
						if (saraBlue is null)
						{
								saraBlue = new Student
								{
										Status = "Student",
										FirstName = "Sara",
										LastName = "Blue",
										Birthday = new DateTime(2006, 01, 05),
										Village = "Village 1",
										Address = "123 W",
										Phone = "258 234 567",
										EnglishLevel = 3,
										ComputerLevel = 4,
								};
								_db.Students.Add(saraBlue);
								saveChanges = true;
						}
						if (cindyAdams is null)
						{
								cindyAdams = new Student
								{
										Status = "Applicant",
										FirstName = "Cindy",
										LastName = "Adams",
										Birthday = new DateTime(2005, 04, 24),
										Village = "Village 2",
										Address = "789 S",
										Phone = "258 123 456"
								};
								_db.Students.Add(cindyAdams);
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
								saraBlueGuardian = new Guardian
								{
										FirstName = "Jessica",
										LastName = "Blue",
										Relationship = "Mother",
										StudentId = saraBlue.Id
								};
								_db.Guardians.Add(saraBlueGuardian);
								saveChanges = true;
						}
						if (cindyAdamsGuardian is null)
						{
								cindyAdamsGuardian = new Guardian
								{
										FirstName = "Tom",
										LastName = "Adams",
										Relationship = "Father",
										StudentId = cindyAdams.Id
								};
								_db.Guardians.Add(cindyAdamsGuardian);
								saveChanges = true;
						}
						if (saveChanges)
								_db.SaveChanges();
				}

				private void SeedRatings(Student student)
				{
						//TODO make it randomly pick from list of ratings

						//TODO filter based on application status? or leave as future students will still have ratings
						//if(student.Status != Role.Status_Pending)
						if (!_db.Ratings.Any(r => r.StudentId == student.Id))
						{
								_db.Ratings.AddRange(
								new Rating
								{
										Age = 16,
										SchoolLevel = 10,
										Academics = 3,
										FoodAssistance = 1,
										AnnualIncome = 200,
										Determination = 5,
										FamilySupport = 1,
										StudentId = student.Id
								}
								);
								_db.SaveChanges();
						}
				}
				private void SeedCourses()
				{
						Instructor cindyReed;
						Instructor samPrice;
						Instructor alicePeterson;
						(cindyReed, samPrice, alicePeterson) = GetInstructors();
						School publicSchool;
						School boanne;
						(publicSchool, boanne) = GetSchools();
						Term fall22 = GetTerm();
						Subject english1;
						Subject computers1;
						(english1, computers1) = GetSubjects();

						if (!_db.Courses.Any())
						{
								_db.Courses.AddRange(
										new Course
										{
												InstructorId = cindyReed.Id,
												SchoolId = boanne.Id,
												TermId = fall22.Id,
												SubjectId = computers1.Id
										}
										);
								_db.SaveChanges();
						}
				}

				private Term GetTerm()
				{
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
								_db.SaveChanges();
						}
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
								cindyReed = new Instructor
								{
										FirstName = "Cindy",
										LastName = "Reed",
										Email = "CindyR@gmail.com",
										Role = "Social Worker"
								};
								_db.Add(cindyReed);
								saveChanges = true;
						}
						if (samPrice is null)
						{
								samPrice = new Instructor
								{
										FirstName = "Sam",
										LastName = "Price",
										Email = "SamP@gmail.com",
										Role = "Admin"
								};
								_db.Add(samPrice);
								saveChanges = true;
						}
						if (alicePeterson is null)
						{
								alicePeterson = new Instructor
								{
										FirstName = "Alice",
										LastName = "Peterson",
										Email = "AliceP@gmail.com",
										Role = "Instructor"
								};
								_db.Add(alicePeterson);
								saveChanges = true;
						}
								if(saveChanges)
										_db.SaveChanges();
						return (cindyReed, samPrice, alicePeterson);
				}
				
				private (Subject, Subject) GetSubjects() 
				{
						bool saveChanges = false;
						Subject? english1 = _db.Subjects.FirstOrDefault(s => s.Name == "English 1");
						Subject? computers1 = _db.Subjects.FirstOrDefault(s => s.Name == "English 1");
						if(english1 is null) 
						{
								english1 = new Subject
								{
										Name = "English 1"
								};
								_db.Add(english1);
								saveChanges = true;
						}
						if (computers1 is null)
						{
								computers1 = new Subject
								{
										Name = "Computers 1"
								};
								_db.Add(computers1);
								saveChanges = true;
						}	
						if(saveChanges)
								_db.SaveChanges();

						return (english1, computers1);
				}

				private (School, School) GetSchools() 
				{
						bool saveChanges = false;
						School? publicSchool = _db.Schools.FirstOrDefault(s => s.Name == "Public");
						School? boanne = _db.Schools.FirstOrDefault(s => s.Name == "Boanne");
						if(publicSchool is null) 
						{
								publicSchool = new School
								{
										Name = "Public School"
								};
								_db.Add(publicSchool);
								saveChanges = true;
						}
						if (boanne is null)
						{
								boanne = new School
								{
										Name = "Boanne"
								};
								_db.Add(boanne);
								saveChanges = true;
						}	
						if(saveChanges)
								_db.SaveChanges();

						return (publicSchool, boanne);
				}
							
		}
}

