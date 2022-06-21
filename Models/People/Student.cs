using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string Village { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int EnglishLevel { get; set; }
        public int ComputerLevel { get; set; }
        [ValidateNever]
        [ForeignKey("Applicant")]
        public Applicant Applicant { get; set; }
        public int ApplicantId { get; set; }
    }
}
