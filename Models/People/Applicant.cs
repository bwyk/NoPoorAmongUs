using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Applicant : Person
		{
				public int Id { get; set; }
				public string Status { get; set; }


                [ValidateNever]
                [ForeignKey("GuardianId")]
                public List<Guardian> Guardians { get; set; }

                [ValidateNever]
                [ForeignKey("RatingsId")]
                public Ratings Ratings { get; set; }
    }
}
