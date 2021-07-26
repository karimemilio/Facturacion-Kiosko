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

        [Display(Name = "Marca")]
        public Marca Marca { get; set; }

        [Display(Name = "MarcaID")]
        //[ForeignKey("MarcaID")]
        public int MarcaID { get; set; }

        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        public DateTime Vencimiento { get; set; }

        [Display(Name = "No vence")]
        public bool NoVence { get; set; }

        [Display(Name = "Precio de costo")]
        public long PrecioCosto { get; set; }

        [Display(Name = "Precio de venta")]
        [Required]
        public long PrecioVenta { get; set; }

        [Display(Name = "Codigo")]
        public int Codigo { get; set; }
        // public List<long> PreciosHistoricos { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}