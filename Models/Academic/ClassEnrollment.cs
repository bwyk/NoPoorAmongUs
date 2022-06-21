using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class ClassEnrollment
    {
        [Key]
        public int Id{ get; set; }
        public int ClassSessionId { get; set; }
        [ForeignKey("ClassSessionId")]
        public ClassSession ClassSession { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        
    }
}
