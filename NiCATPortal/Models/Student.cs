using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiCATPortal.Models
{
    public class Student : ApplicationUser
    {
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
