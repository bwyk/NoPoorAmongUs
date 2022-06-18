using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
		public class Term
		{
				public int Id { get; set; }
				public string Name { get; set; }
				public DateOnly StartDate { get; set; }
				public DateOnly EndDate { get; set; }
				public bool IsActive { get; set; }
		}
}
