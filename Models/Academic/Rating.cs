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
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        //public int? RaterId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Age Rating must be between 1 to 5")]
        public int Age { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Academics Rating must be between 1 to 5")]
        public int Academics { get; set; }
        [Display(Name = "Finances")]
        [Required]
        [Range(1, 5, ErrorMessage = "Income Rating must be between 1 to 5")]
        public int AnnualIncome { get; set; }
        [Display(Name = "Family Support")]
        [Required]
        [Range(1, 5, ErrorMessage = "Support Rating must be between 1 to 5")]
        public int FamilySupport { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Distance Rating must be between 1 to 5")]
        public int Distance { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int StudentId { get; set; }
        [ValidateNever]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}