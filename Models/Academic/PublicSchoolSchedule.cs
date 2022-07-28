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

        //public Weekdays Weekday { get; set; }

        public string Weekday { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }

        [DisplayName("Students")]
        public int StudentId { get; set; }

        [ValidateNever]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }

}
