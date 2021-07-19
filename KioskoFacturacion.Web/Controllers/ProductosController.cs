using System.Collections.Generic;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KioskoFacturacion.Web.Data;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;


namespace KioskoFacturacion.Web.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Productos.Include("Rubro").ToListAsync());
        }

        public IActionResult Alta()
        {
            List<Rubro> rubrosList = _context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Alta([Bind("ID, Nombre, Marca, Descripcion, Vencimiento, NoVence, RubroID, Rubro, PrecioCosto, PrecioVenta, Codigo")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Rubro> rubrosList = _context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return View(producto);
        }

        public async Task<IActionResult> Buscar(string filtroNombre)
        {
            ViewData["filtroNombre"] = filtroNombre;
            List<Producto> productosList;
            if (string.IsNullOrEmpty(filtroNombre))
            {
                productosList = await _context.Productos.Include("Rubro").ToListAsync();
            }
            else
            {
                productosList = await _context.Productos.Where(p => p.Nombre.Contains(filtroNombre)).Include("Rubro").ToListAsync();
                //productosList = await _context.Productos.Where(p => p.Nombre.Contains(filtroNombre)).Include("Rubro").OrderBy(p => p.Nombre).Take(50).ToListAsync();
            }
            return View("Index", productosList);
        }

    }
}
