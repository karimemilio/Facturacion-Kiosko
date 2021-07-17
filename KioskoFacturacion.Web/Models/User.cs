using Microsoft.AspNetCore.Identity;

namespace KioskoFacturacion.Web.Models
{
    public class User : IdentityUser
    {
        public string Nombre { get; set; }
    }
}