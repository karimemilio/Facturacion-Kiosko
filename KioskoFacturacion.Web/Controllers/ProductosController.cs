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
        private readonly int _RegistrosPorPagina = 6;
        private Paginacion<Producto> _PaginadorProductos;

        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter, int pagina = 1)
        {
            int _TotalRegistros = 0;
            // Número total de registros de la tabla Productos
            _TotalRegistros = _context.Productos.Count();
            ViewData["CurrentFilter"] = filter;
            var productos = from e in this._context.Productos
                            .OrderBy(x => x.Nombre)
                            .Skip((pagina - 1) * _RegistrosPorPagina)
                            .Take(_RegistrosPorPagina)
                            .Include("Marca.Rubro")
                            select e;

            // Número total de páginas de la tabla Productos
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores

            if (!String.IsNullOrEmpty(filter))
            {
                productos = productos.Where(e => e.Nombre.Contains(filter) || e.Marca.Nombre.Contains(filter) || e.Marca.Rubro.Nombre.Contains(filter) || e.Codigo.Contains(filter)).OrderBy(x => x.Nombre);
                _TotalRegistros = productos.Count();

            }
            _PaginadorProductos = new Paginacion<Producto>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = productos.ToList()
            };

            return View(_PaginadorProductos);

        }

        public IActionResult Alta()
        {
            var marcasList = _context.Marcas
                .Select(s => new
                {
                    Text = s.Nombre + " (" + s.Rubro.Nombre + ")",
                    Value = s.ID
                })
                .ToList();
            ViewBag.marcasList = new SelectList(marcasList, "Value", "Text");
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

        [HttpPost]
        public async Task<IActionResult> CrearMarca([Bind("ID, Nombre, Codigo, Descripcion, Estado, PrecioCosto, PrecioVenta MarcaID")] Producto producto)
        {
            return RedirectToAction("CreateFromProducto", "Marcas", new { area = "" });
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

        public IActionResult Detail(int id)
        {
            Producto detail = _context.Productos.Include("Marca.Rubro").FirstOrDefault(i => i.ID == id);
            return View(detail);
        }
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.Include("Marca.Rubro")
                .FirstOrDefaultAsync(m => m.ID == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            List<Marca> marcasList = _context.Marcas.Include("Rubro").ToList();
            ViewBag.MarcasList = new SelectList(marcasList, "ID", "Nombre");
            Producto editar = _context.Productos.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }
        public IActionResult Actualizar(int id, string nombre, string estado, int marcaID, string descripcion, float PrecioCosto, float PrecioVenta, string codigo)
        {
            Producto editar = _context.Productos.FirstOrDefault(i => i.ID == id);
            editar.Nombre = nombre;
            editar.Estado = estado;
            editar.MarcaID = marcaID;
            editar.Descripcion = descripcion;
            editar.Codigo = codigo;
            //editar.PreciosCosto = editar.PrecioCosto.Add(precioC);
            editar.PrecioCosto = PrecioCosto;
            //editar.PreciosVenta.Add(precioV);
            editar.PrecioVenta = PrecioVenta;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> BatchDelete(int[] deleteInputs)
        {
            if (deleteInputs != null && deleteInputs.Length > 0)
            {
                var lista = _context.Productos.Include("Marca.Rubro").Where(r => deleteInputs.Contains(r.ID));
                return View(await lista.ToListAsync());
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("BatchConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchConfirmed(int[] deleteInputs)
        {
            var lista = _context.Productos.Where(r => deleteInputs.Contains(r.ID));
            foreach (var item in lista)
            {
                _context.Productos.Remove(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
