using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public  class Assessment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        [ForeignKey("Class")]
        public Class Class { get; set; }
        public int ClassId { get; set; }

    }
}
