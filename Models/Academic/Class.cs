using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		[Keyless]
		public class Days
		{
			public string Day { get; set; }
		}
		public class Class
		{
				[Key]
				public int Id { get; set; }
				public string School { get; set; }
				[NotMapped]
				public List<Days> DaysOfWeek { get; set; }
				public List<Student> EnrolledStudents { get; set; }


				public int InstructorId { get; set; }
				[ValidateNever]
				[ForeignKey("EmployeeId")]
				public Employee Instructor { get; set; }

				[ValidateNever]
				[ForeignKey("TermId")]
				public Term Term { get; set; }
		}
}
