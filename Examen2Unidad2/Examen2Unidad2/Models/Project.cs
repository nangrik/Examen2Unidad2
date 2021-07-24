using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Unidad2.Models
{
    public class Project
    {
        [Key]
        public int projectID { get; set; }
        [StringLength(200, ErrorMessage = "Excede el limite de 200 caracteres")]
        [MinLength(4, ErrorMessage = "Debe tener al menos 4 caracteres")]
        [Display(Name = "Nombre de proyecto")]
        public string projectNAME { get; set; }

        public IEnumerable<TODO> tODOs { get; set; }
        public IEnumerable<DONE> dONEs { get; set; }
        public IEnumerable<DOING> dOINGs { get; set; }
    }
}
