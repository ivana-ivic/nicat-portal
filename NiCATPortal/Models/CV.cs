using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NiCATPortal.Models
{
    public class CV
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Morate da izaberete fajl")]
        [MaxLength(100, ErrorMessage = "Maksimalna dužina naziva fajla je 100 karaktera")]
        [Display(Name = "Fajl")]
        public string FileName { get; set; }

        public virtual Student Student { get; set; }
    }
}
