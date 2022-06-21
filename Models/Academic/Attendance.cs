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
        public int ClassEnrollmentId { get; set; }
        [ForeignKey("ClassEnrollmentId")]
        public ClassEnrollment ClassEnrollment { get; set; }
        
    }
}
