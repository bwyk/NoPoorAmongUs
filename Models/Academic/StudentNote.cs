using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class StudentNote
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string Priority { get; set; }

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        [ValidateNever]
        public Student Student { get; set; }

        public int NoteTypeId { get; set; }

        [ForeignKey("NoteTypeId")]
        [ValidateNever]
        [Display(Name = "Note Type")]
        public NoteType NoteType { get; set; }

        [ValidateNever]
        [Display(Name = "User")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}