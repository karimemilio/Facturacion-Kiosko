using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KioskoFacturacion.Web.Models
{
    public class Paginacion<T> where T : class
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }

        public int TotalRegistros { get; set; }

        public int TotalPaginas { get; set; }

        public IEnumerable<T> Resultado { get; set; }
    }
}
