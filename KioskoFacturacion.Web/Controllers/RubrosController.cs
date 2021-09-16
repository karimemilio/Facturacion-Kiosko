using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using KioskoFacturacion.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Data;

namespace KioskoFacturacion.Web.Controllers
{

    [Authorize]
    public class RubrosController : Controller
    {
        private ApplicationDbContext context;
        public RubrosController(ApplicationDbContext context)
        {
            this.context = context;
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
            ViewData["StateSortParm"] = sortOrder == "State" ? "state_desc" : "State";

            //Obtencion de todos los rubros
            var rubros = from e in this.context.Rubros
                         select e;

            //Ordenación
            switch (sortOrder)
            {
                case "name_desc":
                    rubros = rubros.OrderByDescending(r => r.Nombre);
                    break;
                case "State":
                    rubros = rubros.OrderBy(s => s.Estado);
                    break;
                case "state_desc":
                    rubros = rubros.OrderByDescending(s => s.Estado);
                    break;
                default:
                    rubros = rubros.OrderBy(r => r.Nombre);
                    break;
            }

            int pageSize = 6;

            return View(await PaginatedList<Rubro>.CreateAsync(rubros.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateFromMarca()
        {
            return View();
        }
        public IActionResult CreateFromProducto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([Bind("Nombre, Estado")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = context.Rubros.FirstOrDefault(x => x.Nombre == rubro.Nombre);
                if (nom == null)
                {
                    context.Add(rubro);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction(nameof(Create));
        }

        [HttpPost]
        public async Task<IActionResult> GuardarFromMarca([Bind("Nombre, Estado")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = context.Rubros.FirstOrDefault(x => x.Nombre == rubro.Nombre);
                if (nom == null)
                {
                    context.Add(rubro);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Create", "Marcas", new { area = "" });
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction(nameof(CreateFromMarca));
        }
        [HttpPost]
        public async Task<IActionResult> GuardarFromProducto([Bind("Nombre, Estado")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = context.Rubros.FirstOrDefault(x => x.Nombre == rubro.Nombre);
                if (nom == null)
                {
                    context.Add(rubro);
                    await context.SaveChangesAsync();
                    return RedirectToAction("CreateFromProducto", "Marcas", new { area = "" });
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction(nameof(CreateFromMarca));
        }
        public IActionResult Edit(int id)
        {
            Rubro editar = context.Rubros.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }
        public IActionResult Detail(int id)
        {
            Rubro detail = context.Rubros.FirstOrDefault(i => i.ID == id);
            return View(detail);
        }
        public IActionResult Actualizar(int id, string nombre, string estado)
        {
            if (ModelState.IsValid)
            {
                Rubro editar = context.Rubros.FirstOrDefault(i => i.ID == id);
                var nom = context.Rubros.FirstOrDefault(x => x.Nombre == nombre);
                if ((nom == null) || (nombre == editar.Nombre))
                {
                    editar.Nombre = nombre;
                    editar.Estado = estado;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction(nameof(Edit));

        }

        public IActionResult Nombre(string nombre)
        {
            ViewData["rubroNombre"] = nombre;
            ViewBag.RubroNombre = nombre;
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubro = await context.Rubros
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rubro == null)
            {
                return NotFound();
            }

            return View(rubro);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rubro = await context.Rubros.FindAsync(id);
            context.Rubros.Remove(rubro);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BatchDelete(int[] deleteInputs)
        {
            if (deleteInputs != null && deleteInputs.Length > 0)
            {
                var lista = context.Rubros.Where(r => deleteInputs.Contains(r.ID));
                return View(await lista.ToListAsync());
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("BatchConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchConfirmed(int[] deleteInputs)
        {
            var lista = context.Rubros.Where(r => deleteInputs.Contains(r.ID));
            foreach (var item in lista)
            {
                context.Rubros.Remove(item);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

