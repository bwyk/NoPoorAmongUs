using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class ClassSession
    {
        public int Id { get; set; }
        //Does the day need to be a different data type?
        public string Day { get; set; }
        [ForeignKey("Class")]
        public Class Class { get; set; }
        public int ClassId { get; set; }
    }
}
