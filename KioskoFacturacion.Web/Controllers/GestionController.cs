using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace KioskoFacturacion.Web.Controllers
{
    [Authorize]

    public class GestionController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ModificarProducto()
        {
            return View();
        }

        public IActionResult Rubros()
        {
            return View();
        }

        public IActionResult Listados()
        {
            return View();
        }
        public IActionResult GuardarProducto()
        {
            Producto nuevoProducto = new Producto();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
