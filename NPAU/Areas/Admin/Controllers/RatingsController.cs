using Microsoft.AspNetCore.Mvc;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Dynamic;
using Models.ViewModels;
using System.Diagnostics;
using System.Data;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class RatingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RatingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public ViewResult Index()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Age", typeof(double));
            dt.Columns.Add("Academics", typeof(double));
            dt.Columns.Add("Finances", typeof(double));
            dt.Columns.Add("Support", typeof(double));
            dt.Columns.Add("Distance", typeof(double));
            dt.Columns.Add("Average", typeof(double));

            IEnumerable<Rating> Ratings = _unitOfWork.Rating.GetAll();
            IEnumerable<Student> Students = _unitOfWork.Student.GetAll(s => s.Status == "Pending");

            double counter = 0.0;
            //This number is for the amount of ratings we have can be scaled up or down.
            //For calculation purposes
            double constRatings = 5;
            //Age
            double sumAge = 0.0;
            double avgAge = 0.0;
            //Academics
            double sumAcademics = 0.0;
            double avgAcdemics = 0.0;
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

            foreach (var student in Students)
            {
                DataRow row = dt.NewRow();
                foreach (var rating in Ratings)
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
                avgAcdemics = sumAcademics / counter;
                avgFinances = sumFinances / counter;
                avgSupport = sumSupport / counter;
                avgDistance = sumDistance / counter;

                avgScore = (avgAge + avgAcdemics + avgFinances + avgSupport + avgDistance) / constRatings;

                row["Name"] = student.FullName;
                row["Age"] = avgAge;
                row["Academics"] = avgAcdemics;
                row["Finances"] = avgFinances;
                row["Support"] = avgSupport;
                row["Distance"] = avgDistance;
                row["Average"] = avgScore;

                dt.Rows.Add(row);
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "Average DESC";
            dt = dv.ToTable();



            return View(dt);
        }

    }
}