using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KioskoFacturacion.Web.Models;

namespace KioskoFacturacion.Web.Controllers
{
    public class ListadosController : Controller
    {

        List<Rubro> Data = null;

        public ListadosController()
        {
            Data = new List<Rubro>();
            Data.Add(new Rubro("Perfumeria", "Activo"));
            Data.Add(new Rubro("Regaleria", "Activo"));
            Data.Add(new Rubro("Libreria", "Activo"));
            Data.Add(new Rubro("Golosinas", "Activo"));
        }

        public IActionResult Index()
        {
            return View(Data);
        }

        public IActionResult ListarProductos()
        {
            return View();
        }

        public IActionResult Listar(string nombre, int cantidad)
        {

            return View(nombre);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
