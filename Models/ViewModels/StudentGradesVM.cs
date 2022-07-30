using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class StudentGradeVM
    {
        public StudentGradeVM(Models.Student student, int currentScore, int maxScore, int gradeId, string currentComment)
        {
            Student = student;
            CurrentScore = currentScore;
            MaxScore = maxScore;
            GradeId = gradeId;
            CurrentComment = currentComment;
        }

        public Models.Student Student { get; set; }
        public int CurrentScore { get; set; }
        public string CurrentComment { get; set; }
        public int MaxScore { get; set; }
        public int GradeId { get; set; }
    }
}
