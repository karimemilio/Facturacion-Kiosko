using System.Collections.Generic;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using KioskoFacturacion.Web.Data;

namespace KioskoFacturacion.Web.Controllers
{
    public class RubrosController : Controller
    {
        private ApplicationDbContext context;
        public RubrosController(ApplicationDbContext context)
        {
            this.context = context;

        }
        public IActionResult Index()
        {
            return View(context.Rubros.ToList());
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        public IActionResult Guardar(string nombre, string estado)
        {
            Rubro nuevo = new Rubro();
            nuevo.Nombre = nombre;
            nuevo.Estado = estado;

            context.Rubros.Add(nuevo);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            Rubro editar = context.Rubros.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }

        public IActionResult Actualizar(int id, string nombre, string estado)
        {
            Rubro editar = context.Rubros.FirstOrDefault(i => i.ID == id);
            editar.Nombre = nombre;
            editar.Estado = estado;
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Nombre(string nombre)
        {
            ViewData["rubroNombre"] = nombre;
            ViewBag.RubroNombre = nombre;
            return View();
        }
    }
}