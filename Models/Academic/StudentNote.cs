using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Academic
{
    public class StudentNote
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [ForeignKey("Student")]
        public Student Student { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("NoteType")]
        public NoteType NoteType { get; set; }
        public int NoteTypeId { get; set; }
    }
}
