using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
		public class Student : Applicant
		{
				public int StudentId { get; set; }

				public int EnglishLevel { get; set; }
				public int ComputerLevel { get; set; }
		}
}
