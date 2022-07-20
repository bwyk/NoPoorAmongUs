using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class CourseEnrollment
    {
        [Key]
        public int Id{ get; set; }
        public int CourseSessionId { get; set; }
        [ForeignKey("CourseSessionId")]
        public CourseSession? CourseSession { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
        
    }
}
