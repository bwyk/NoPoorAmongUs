﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Models.Academic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Instructor")]
        public int InstructorId { get; set; }
        [ValidateNever]
        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }

        [DisplayName("Term")]
        public int TermId { get; set; }
        [ValidateNever]
        [ForeignKey("TermId")]
        public Term Term { get; set; }

        [DisplayName("School")]
        public int SchoolId { get; set; }
        [ValidateNever]
        [ForeignKey("SchoolId")]
        public School School { get; set; }

        [DisplayName("Subject")]
        public int SubjectId { get; set; }
        [ValidateNever]
        [ForeignKey("SubjectId")]
        public Subject Subject { get; set; }
        
    }
}
