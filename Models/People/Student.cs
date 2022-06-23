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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string Village { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int EnglishLevel { get; set; }
        public int ComputerLevel { get; set; }

        public int ApplicantId { get; set; }
    }
}
