using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [Required]
        public string Village { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DisplayName("English Level")]
        public int EnglishLevel { get; set; }
        [Required]
        [DisplayName("Computer Level")]
        public int ComputerLevel { get; set; }
    }
}
