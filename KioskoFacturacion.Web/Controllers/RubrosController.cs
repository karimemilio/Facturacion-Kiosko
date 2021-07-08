using System.Collections.Generic;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KioskoFacturacion.Web.Controllers
{
    public class RubrosController : Controller
    {
        List<Rubro> Data = null;

        public RubrosController()
        {
            Data = new List<Rubro>();
            Data.Add(new Rubro() { ID = Guid.Parse("af5da2a1-a9e4-4b57-b9e5-5374a38a1fa2"), Nombre = "Golosinas", Estado = "Activo" });
            Data.Add(new Rubro() { ID = Guid.Parse("bf5da2a1-a9e4-4b57-b9e5-5374a38a1fa2"), Nombre = "Cigarrillos", Estado = "Activo" });
            Data.Add(new Rubro() { ID = Guid.Parse("cf5da2a1-a9e4-4b57-b9e5-5374a38a1fa2"), Nombre = "Limpieza", Estado = "Activo" });
            Data.Add(new Rubro() { ID = Guid.Parse("df5da2a1-a9e4-4b57-b9e5-5374a38a1fa2"), Nombre = "Fotocopias", Estado = "Inactivo" });
            Data.Add(new Rubro() { ID = Guid.Parse("ef5da2a1-a9e4-4b57-b9e5-5374a38a1fa2"), Nombre = "Bebidas", Estado = "Activo" });
        }
        public IActionResult Index()
        {
            return View(Data);
        }
        
        public IActionResult Nuevo(string nombre, string estado)
        {
            Rubro nuevo = new Rubro();
            nuevo.ID = Guid.NewGuid();
            nuevo.Nombre = nombre;
            nuevo.Estado = estado;
            Data.Add(nuevo);
            return View();
        }

        public IActionResult Editar(Guid id)
        {
            Rubro editar = Data.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }

        public IActionResult Actualizar(Guid id, string nombre, string estado)
        {
            Rubro editar = Data.FirstOrDefault(i => i.ID == id);
            editar.Nombre = nombre;
            editar.Estado = estado;
            return View("Index", Data);
        }

        public IActionResult Nombre(string nombre)
        {
            ViewData["rubroNombre"] = nombre;
            ViewBag.RubroNombre = nombre;
            return View();
        }
    }
}