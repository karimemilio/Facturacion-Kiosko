using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskoFacturacion.Web.Models
{
    public class Producto
    {
        [MaxLength(100)]
        [Display(Name = "Producto")]
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Descripcion { get; set; }
        public DateTime Vencimiento { get; set; }
        public bool NoVence { get; set; }
        public string Rubro { get; set; }
        public long PrecioCosto { get; set; }
        public long PrecioVenta { get; set; }
        [Key]
        public int codigo { get; set;}
    }
}