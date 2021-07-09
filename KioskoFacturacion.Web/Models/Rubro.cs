using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskoFacturacion.Web.Models
{
    public class Rubro
    {
        public Rubro()
        {

        }
        public Rubro(string nombre, string estado)
        {
            Nombre = nombre;
            Estado = estado;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Nombre { get; set; }

        public string Estado { get; set; }
    }
}