using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public  class Assessment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }
        [ValidateNever]
        [ForeignKey("ClassId")]
        public Course Class { get; set; }
        

    }
}
