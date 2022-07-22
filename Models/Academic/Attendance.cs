using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public bool Excused { get; set; }
        public int ClassEnrollmentId { get; set; }
        [ForeignKey("ClassEnrollmentId")]
        public CourseEnrollment ClassEnrollment { get; set; }
        
    }
}
