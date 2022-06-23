using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Guardian
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }

        public int ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        public Applicant Applicant { get; set; }
        

    }
}
