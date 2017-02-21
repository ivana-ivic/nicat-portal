using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiCATPortal.Models
{
    public class Administrator : ApplicationUser
    {
    }

    public enum UserType
    {
        Student,
        Profesor
    }
}
