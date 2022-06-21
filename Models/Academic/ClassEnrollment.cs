using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class ClassEnrollment
    {
        public int Id{ get; set; }
        [ForeignKey("ClassSession")]
        public ClassSession ClassSession { get; set; }
        public int ClassSessionId { get; set; }
        [ForeignKey("Student")]
        public Student Student { get; set; }
        public int StudentId { get; set; }
    }
}
