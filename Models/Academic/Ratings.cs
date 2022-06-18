using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Ratings
		{
				public int Id { get; set; }
				public int Age { get; set; }
				public int SchoolLevel { get; set; }
				public int Academics { get; set; }
				public int FoodAssistance { get; set; }
				public int AnnualIncome { get; set; }
				public int Determination { get; set; }
				public int FamilySupport { get; set; }

				[ValidateNever]
				[ForeignKey("ApplicantId")]
				public Applicant Applicant { get; set; }
		}
}
