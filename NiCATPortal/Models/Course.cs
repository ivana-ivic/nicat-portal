using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NiCATPortal.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Morate uneti naziv kursa")]
        [MaxLength(50, ErrorMessage = "Maksimalna dužina naziva je 50 karaktera")]
        [Display(Name = "Naziv kursa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Morate uneti godinu održavanja kursa")]
        [Display(Name = "Godina")]
        [Range(2014, 2050, ErrorMessage = "Godina kursa mora biti u opsegu od 2014. do 2050.")]
        public int Year { get; set; }

        [Display(Name = "Opis kursa")]
        public string Description { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Literature> Literatures { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
    }
}
