using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Unidad2.Models
{
    public class DOING
    {
        [Key]
        public int doingID { get; set; }

        [StringLength(200, ErrorMessage = "Excede el limite de 200 caracteres")]
        [MinLength(4, ErrorMessage = "Debe tener al menos 4 caracteres")]
        [Display(Name = "Nombre de Tarea")]
        public string doingName { get; set; }

        [StringLength(20, ErrorMessage = "Excede el limite de 20 caracteres")]
        [MinLength(2, ErrorMessage = "Debe tener al menos 4 caracteres")]
        [Display(Name = "Estado")]
        public string doingSTATUS { get; set; }


    }
}
