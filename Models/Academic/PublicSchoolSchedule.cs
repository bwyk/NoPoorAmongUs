using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class PublicSchoolSchedule
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("School")]
        public int SchoolId { get; set; }

        [ValidateNever]
        [ForeignKey("SchoolId")]
        public School School { get; set; }

        [DisplayName("Monday")]
        public bool Monday { get; set; } = false;

        [DisplayName("Tuesday")]
        public bool Tuesday { get; set; } = false;

        [DisplayName("Wednesday")]
        public bool Wednesday { get; set; } = false;

        [DisplayName("Thursday")]
        public bool Thursday { get; set; } = false;

        [DisplayName("Friday")]
        public bool Friday { get; set; } = false;

        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }

        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }

        [DisplayName("Students")]
        public int StudentId { get; set; }

        [ValidateNever]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
