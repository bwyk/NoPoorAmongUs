using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class StudentDoc
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [ForeignKey("DocType")]
        public DocType DocType { get; set; }
        public int DocTypeId { get; set; }
    }
}
