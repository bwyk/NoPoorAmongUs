using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Guardian
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Relationship { get; set; }

        [DisplayName("Student ID")]
        public int StudentId { get; set; }
        [ValidateNever]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
