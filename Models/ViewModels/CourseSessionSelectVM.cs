using Models.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class CourseSessionSelectVM
    {
        public int CourseId { get; set; }
        public List<CourseSession> SessionList { get; set; }

    }
}
