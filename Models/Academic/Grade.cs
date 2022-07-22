using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public int AssessmentId { get; set; }
        [ForeignKey("AssessmentId")]
        public Assessment Assessment { get; set; }

        public int? CourseEnrollmentId { get; set; }
        [ForeignKey("CourseEnrollmentId")]
        public CourseEnrollment? CourseEnrollment { get; set; }

    }
}
