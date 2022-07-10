using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudentDoc
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [ValidateNever]
        [Display(Name = "Document")]
        public string DocUrl { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        [ValidateNever]
        public Student Student { get; set; }
        public int DocTypeId { get; set; }
        [ForeignKey("DocTypeId")]
        [ValidateNever]
        public DocType DocType { get; set; }
        
    }
}
