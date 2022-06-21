using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class Attendance
    {
        public int Id { get; set; }
        [ForeignKey("ClassEnrollment")]
        public ClassEnrollment ClassEnrollment { get; set; }
        public int ClassEnrollmentId { get; set; }
    }
}
