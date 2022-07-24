using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class RatingVM
    {
        //public IEnumerable<Rating> RatingList { get; set; }
        //public Student Student { get; set; }
        //public IEnumerable<Student> StudentList { get; set; }

        //public List<RatingsObj> RatingsObjs { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public double AvgAge { get; set; }
        public double AvgAcademics { get; set; }
        public double AvgFinances { get; set; }
        public double AvgSupport { get; set; }
        public double AvgDistance { get; set; }
        public double AvgScore { get; set; }
        public bool accepted { get; set; }
        //public bool rejected { get; set; }

    }
}