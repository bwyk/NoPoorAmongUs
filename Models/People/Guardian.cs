using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Guardian : Person
		{
				public string Relationship { get; set; }
				public Applicant Dependent { get; set; }
		}
}
