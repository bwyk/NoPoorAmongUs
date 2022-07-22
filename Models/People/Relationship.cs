using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.People
{
    public class Relationship
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Relationship Type")]
        public string RelationshipType { get; set; }
        [DisplayName("Student ID")]
        public int StudentId { get; set; }
        [ValidateNever]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }
        public int GuardianId { get; set; }
        [ValidateNever]
        [ForeignKey("GuardianId")]
        public Guardian Guardian { get; set; }
    }
}