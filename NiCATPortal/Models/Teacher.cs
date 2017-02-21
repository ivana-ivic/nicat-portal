using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiCATPortal.Models
{
    public class Teacher : ApplicationUser
    {
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Literature> Literature { get; set; }
    }
}
