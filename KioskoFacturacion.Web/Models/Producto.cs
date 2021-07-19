using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskoFacturacion.Web.Models
{
    public class Producto
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Producto")]
        public string Nombre { get; set; }

        [Required]
        public string Marca { get; set; }

        public string Descripcion { get; set; }

        public DateTime Vencimiento { get; set; }

        [Display(Name = "No vence")]
        public bool NoVence { get; set; }

        [Display(Name = "RubroID")]
        [Required]
        public int RubroID { get; set; }

        [Display(Name = "Rubro")]
        //[ForeignKey("RubroID")]
        public Rubro Rubro { get; set; }

        [Display(Name = "Precio de costo")]
        public long PrecioCosto { get; set; }

        [Display(Name = "Precio de venta")]
        [Required]
        public long PrecioVenta { get; set; }
        [Key]
        [Display(Name = "Codigo")]
        public int Codigo { get; set; }
        // public List<long> PreciosHistoricos { get; set; }
    }
}