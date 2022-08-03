using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class SessionAttendance
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateTaken { get; set; } = DateTime.Now.Date;
        //public List<Attendance> Attendances { get; set; }
        public int CourseSessionId { get; set; }
        [ForeignKey("CourseSessionId")]
        public CourseSession? CourseSession { get; set; }
    }
}
