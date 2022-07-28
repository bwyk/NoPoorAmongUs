using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Models.ViewModels;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class RatingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RatingVM ratingVM { get; set; }


        public RatingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            //RatingsObjList = new RatingsObj();
            List<RatingVM> ratingVMList = new List<RatingVM>();

            //ratingVM = new RatingVM()
            //{
            //    StudentList = _unitOfWork.Student.GetAll(s => s.Status == "Pending"),
            //    RatingList = _unitOfWork.Rating.GetAll(),
            //   //RatingsObjs = new()
            //};

            double counter = 0.0;
            //This number is for the amount of ratings we have can be scaled up or down.
            //For calculation purposes
            double constRatings = 5;
            //Age
            double sumAge = 0.0;
            double avgAge = 0.0;
            //Academics
            double sumAcademics = 0.0;
            double avgAcademics = 0.0;
            //Finances
            double sumFinances = 0.0;
            double avgFinances = 0.0;
            //Support
            double sumSupport = 0.0;
            double avgSupport = 0.0;
            //Distance
            double sumDistance = 0.0;
            double avgDistance = 0.0;
            //Avg Score            
            double avgScore = 0.0;




            IEnumerable<Rating> ratingsList = _unitOfWork.Rating.GetAll();
            IEnumerable<Student> studentList = _unitOfWork.Student.GetAll(s => s.Status == "Pending");


            foreach (var student in studentList)
            {
                RatingVM obj = new RatingVM();
                foreach (var rating in ratingsList)
                {
                    if (rating.StudentId == student.Id)
                    {
                        counter++;
                        sumAge += rating.Age;
                        sumAcademics += rating.Academics;
                        sumFinances += rating.AnnualIncome;
                        sumSupport += rating.FamilySupport;
                        sumDistance += rating.Distance;

                    }

                }
                avgAge = sumAge / counter;
                avgAcademics = sumAcademics / counter;
                avgFinances = sumFinances / counter;
                avgSupport = sumSupport / counter;
                avgDistance = sumDistance / counter;

                avgScore = (avgAge + avgAcademics + avgFinances + avgSupport + avgDistance) / constRatings;

                obj.Id = student.Id;
                obj.Name = student.FullName;
                obj.AvgAge = Math.Round(avgAge, 2);
                obj.AvgAcademics = Math.Round(avgAcademics, 2);
                obj.AvgFinances = Math.Round(avgFinances, 2);
                obj.AvgSupport = Math.Round(avgSupport, 2);
                obj.AvgDistance = Math.Round(avgDistance, 2);
                obj.AvgScore = Math.Round(avgScore, 2);
                obj.accepted = false;

                ratingVMList.Add(obj);
            }
            var sortedList = ratingVMList.OrderByDescending(s => s.AvgScore).ToList();
            ratingVMList = sortedList;



            return View(ratingVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<RatingVM> items)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in items)
                {
                    if (item.accepted == true)
                    {
                        var obj = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == item.Id);
                        obj.Status = "Student";
                        _unitOfWork.Student.Update(obj);
                        _unitOfWork.Save();
                    }
                }
                TempData["success"] = "Students Accepted";
                return RedirectToAction("Index");
            }
            return View(items);
        }


    }
}