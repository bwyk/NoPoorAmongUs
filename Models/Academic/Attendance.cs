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
        public string MarkedAttendance { get; set; } = string.Empty;
        public bool Excused { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTaken { get; set; } = DateTime.Now;
        public int CourseEnrollmentId { get; set; }
        [ForeignKey("CourseEnrollmentId")]
        public CourseEnrollment CourseEnrollment { get; set; }

        public int SessionAttendanceId { get; set; }

    }
}
