using System;

namespace KioskoFacturacion.Web.Models
{
    public class Rubro
    {
        public Rubro (){
            
        }
        public Rubro(string nombre, string estado)
        {
            Nombre = nombre;
            Estado = estado;
        }

        public Guid ID { get; set;}

        public string Nombre { get; set; }

        public string Estado { get; set; }
    }
}