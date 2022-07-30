using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class StudentAssessmentGradesVM
    {
        public Student Student { get; set; }
        public Course Course { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<Grade> Grades { get; set; }
        
    }
}
