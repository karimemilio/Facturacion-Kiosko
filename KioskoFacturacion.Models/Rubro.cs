namespace KioskoFacturacion.Web.Models
{
    public class Rubro
    {
        public Rubro(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}