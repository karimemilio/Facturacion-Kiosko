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
    public class FacturacionController : Controller
    {
        private readonly int _RegistrosPorPagina = 6;
        private Paginacion<Producto> _PaginadorProductos;

        private readonly ApplicationDbContext _context;

        public FacturacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter, int pagina = 1)
        {
            var productosList = _context.Productos
                .Select(s => new
                {
                    Text = s.Nombre + " (" + s.Marca.Nombre + ")",
                    Value = s.ID
                })
                .ToList();
            ViewBag.productosList = new SelectList(productosList, "Value", "Text");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Alta([Bind("ID, Codigo, Nombre, MarcaID, Descripcion, PrecioCosto, Estado, PrecioVenta")] Producto producto)
        {
            if ((ModelState.IsValid) && (producto.PrecioVenta >= producto.PrecioCosto))
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var marcasList = _context.Marcas
                .Select(s => new
                {
                    Text = s.Nombre + " (" + s.Rubro.Nombre + ")",
                    Value = s.ID
                })
                .ToList();
            ViewBag.marcasList = new SelectList(marcasList, "Value", "Text");
            return View("Alta", producto);
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

