using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NiCATPortal.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Morate da unesete ocenu")]
        [Range(1, 3, ErrorMessage = "Ocena može biti: 1 - student nije položio kurs, 2 - student je položio kurs, 3 - student je položio kurs sa izuzetnim uspehom")]
        [Display(Name = "Fajl")]
        public int Mark { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
