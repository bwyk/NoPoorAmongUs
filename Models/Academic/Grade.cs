using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class Grade
    {
        public int Id { get; set; }
        public int Score { get; set; }
        [ForeignKey("Assessment")]
        public Assessment Assessment { get; set; }
        public int AssessmentId { get; set; }
        [ForeignKey("ClassEnrollment")]
        public ClassEnrollment ClassEnrollment { get; set; }
        public int ClassEnrollmentId { get; set; }

    }
}
