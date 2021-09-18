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

        public async Task<IActionResult> Index(string filter, string sortOrder, string currentFilter, int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;

            if (filter != null)
            {
                pageNumber = 1;
            }
            else
            {
                filter = currentFilter;
            }

            //Filtro de busqueda
            ViewData["CurrentFilter"] = filter;

            //Filtros de ordenamiento
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CodeSortParm"] = sortOrder == "Code" ? "code_desc" : "Code";
            ViewData["MarcaSortParm"] = sortOrder == "Marca" ? "marca_desc" : "Marca";
            ViewData["RubroSortParm"] = sortOrder == "Rubro" ? "rubro_desc" : "Rubro";
            ViewData["StateSortParm"] = sortOrder == "State" ? "state_desc" : "State";

            //Obtencion de todos las marcas
            var productos = from e in this._context.Productos
                         .Include("Marca.Rubro")
                            select e;

            //Ordenación
            switch (sortOrder)
            {
                case "name_desc":
                    productos = productos.OrderByDescending(r => r.Nombre);
                    break;
                case "Code":
                    productos = productos.OrderBy(s => s.Codigo);
                    break;
                case "code_desc":
                    productos = productos.OrderByDescending(s => s.Codigo);
                    break;
                case "Marca":
                    productos = productos.OrderBy(s => s.Marca.Nombre);
                    break;
                case "marca_desc":
                    productos = productos.OrderByDescending(s => s.Marca.Nombre);
                    break;
                case "Rubro":
                    productos = productos.OrderBy(s => s.Marca.Rubro);
                    break;
                case "rubro_desc":
                    productos = productos.OrderByDescending(s => s.Marca.Rubro);
                    break;
                case "State":
                    productos = productos.OrderBy(s => s.Estado);
                    break;
                case "state_desc":
                    productos = productos.OrderByDescending(s => s.Estado);
                    break;
                default:
                    productos = productos.OrderBy(r => r.Nombre);
                    break;
            }

            int pageSize = 6;

            return View(await PaginatedList<Producto>.CreateAsync(productos.AsNoTracking(), pageNumber ?? 1, pageSize));
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

            List<Rubro> rubrosList = _context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");

            return View(new Producto());
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
            List<Rubro> rubrosList = _context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");

            return View(new Producto());
        }

        [HttpPost]
        public async Task<IActionResult> CrearMarca([Bind("Nombre, Estado, RubroID")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = _context.Marcas.FirstOrDefault(x => x.Nombre == marca.Nombre);
                if (nom == null)
                {
                    _context.Add(marca);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Alta", "Productos", new { area = "" });
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction("Alta", "Productos", new { area = "" });
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

        public IActionResult Delete()
        {
            var productos = from e in this._context.Productos
                            .Include("Marca.Rubro")
                            select e;
            return View(productos);
        }

    }
}
