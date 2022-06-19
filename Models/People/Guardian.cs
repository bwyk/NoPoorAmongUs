using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Guardian
		{
				public int Id { get; set; }
				public string Name { get; set; }
				public string Relationship { get; set; }
				public Applicant Dependent { get; set; }
		}
}
