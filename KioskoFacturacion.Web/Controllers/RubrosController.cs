using Microsoft.AspNetCore.Mvc;


namespace KioskoFacturacion.Web.Controllers
{
    public class RubrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Guardar(string nombre, string descripcion)
        {
            ViewBag.Nombre = nombre;
            ViewBag.Descripcion = descripcion;
            // Rubro nuevo = new Rubro();
            //  nuevo.Rubro = descripcion;
            return View();
        }

        public IActionResult Nombre(string nombre)
        {
            ViewData["rubroNombre"] = nombre;
            ViewBag.RubroNombre = nombre;
            return View();
        }
    }
}