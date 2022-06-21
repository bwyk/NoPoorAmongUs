using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class StudentDoc
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int DocTypeId { get; set; }
        [ForeignKey("DocTypeId")]
        public DocType DocType { get; set; }
        
    }
}
