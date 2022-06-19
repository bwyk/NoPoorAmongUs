using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Applicant
		{
				public int Id { get; set; }
				public string Status { get; set; }

				public string Name { get; set; }
				public string Birthday { get; set; }
				public string Village { get; set; }
				public string Address { get; set; }
				public string Phone { get; set; }
				[ValidateNever]
                [ForeignKey("GuardianId")]
                public List<Guardian> Guardians { get; set; }

                [ValidateNever]
                [ForeignKey("RatingsId")]
                public Ratings Ratings { get; set; }
    }
}
