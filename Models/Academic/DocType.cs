using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DocType
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Document Type Name")]
        public string TypeName { get; set; }
        
    }
}
