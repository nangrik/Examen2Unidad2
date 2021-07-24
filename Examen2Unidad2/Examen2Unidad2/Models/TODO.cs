using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Unidad2.Models
{
    public class TODO
    {
        [Key]
        public int TODOid { get; set; }
        [StringLength(200, ErrorMessage = "Excede el limite de 200 caracteres")]
        [MinLength(4, ErrorMessage = "Debe tener al menos 4 caracteres")]
        [Display(Name = "Nombre de Tarea")]
        public string TODOName { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime TODOstart { get; set; }

        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime TODOfinish { get; set; }

        public int projectID { get; set; }
        [ForeignKey("projectID")]
        public Project project { get; set; }
    }
}
