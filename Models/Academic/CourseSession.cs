using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class CourseSession
    {
        [Key]
        public int Id { get; set; }
        //Does the day need to be a different data type?
        public string Day { get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public string CourseName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        
    }
}
