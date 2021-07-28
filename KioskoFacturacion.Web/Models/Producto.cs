using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskoFacturacion.Web.Models
{
    public class Producto
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public uint Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }

        public Marca Marca { get; set; }

        [Required]
        //[ForeignKey("MarcaID")]
        public int MarcaID { get; set; }

        public string Descripcion { get; set; }

        public string Estado { get; set; }

        /*[DataType(DataType.Date)]
       public DateTime Vencimiento { get; set; }

       [Display(Name = "No vence")]
       public bool NoVence { get; set; }
*/
        [Display(Name = "Precio de costo")]
        public float PrecioCosto { get; set; }
        [NotMapped]
        public List<float> PreciosCosto { get; set; }

        [Display(Name = "Precio de venta")]
        [Required]
        public float PrecioVenta { get; set; }
        [NotMapped]
        public List<float> PreciosVenta { get; set; }
    }
}