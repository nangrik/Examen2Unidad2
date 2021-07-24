using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen2Unidad2.Common
{
    public class Pagination<T> where T : class
    {
        //para hacer paginacion
        public int CurrentPage { get; set; }
        public int RecordPerPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPage { get; set; }

        //busquedas
        public string Search { get; set; }

        public List<T> project { get; set; }

        public IEnumerable<T> Result { get; set; }
    }
}
