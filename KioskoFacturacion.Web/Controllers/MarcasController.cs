using System.Collections.Generic;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using KioskoFacturacion.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KioskoFacturacion.Web.Controllers
{
    [Authorize]
    public class MarcasController : Controller
    {
        private ApplicationDbContext context;

        public MarcasController(ApplicationDbContext context)
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
            ViewData["RubroSortParm"] = sortOrder == "Rubro" ? "rubro_desc" : "Rubro";
            ViewData["StateSortParm"] = sortOrder == "State" ? "state_desc" : "State";

            //Obtencion de todos las marcas
            var marcas = from e in this.context.Marcas
                         .Include("Rubro")
                         select e;

            //Ordenación
            switch (sortOrder)
            {
                case "name_desc":
                    marcas = marcas.OrderByDescending(r => r.Nombre);
                    break;
                case "Rubro":
                    marcas = marcas.OrderBy(s => s.Rubro);
                    break;
                case "rubro_desc":
                    marcas = marcas.OrderByDescending(s => s.Rubro);
                    break;
                case "State":
                    marcas = marcas.OrderBy(s => s.Estado);
                    break;
                case "state_desc":
                    marcas = marcas.OrderByDescending(s => s.Estado);
                    break;
                default:
                    marcas = marcas.OrderBy(r => r.Nombre);
                    break;
            }

            int pageSize = 6;

            return View(await PaginatedList<Marca>.CreateAsync(marcas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public IActionResult Create()
        {
            List<Rubro> rubrosList = context.Rubros.ToList();
            //Rubro unRubro = new();
            //unRubro.Nombre = "RUBRO NUEVO";
            //rubrosList.Insert(0, unRubro);
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearRubro([Bind("ID, Nombre, Estado, RubroID")] Marca marca)
        {
            return RedirectToAction("CreateFromMarca", "Rubros", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> CrearRubroFromProducto([Bind("ID, Nombre, Estado, RubroID")] Marca marca)
        {
            return RedirectToAction("CreateFromProducto", "Rubros", new { area = "" });
        }
        public IActionResult CreateFromProducto()
        {
            List<Rubro> rubrosList = context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarFromProducto([Bind("Nombre, Estado, RubroID")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = context.Marcas.FirstOrDefault(x => x.Nombre == marca.Nombre);
                if (nom == null)
                {
                    context.Add(marca);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Alta", "Productos", new { area = "" });
                }
                else
                {
                    string msg = "Nombre duplicado";
                    TempData["ErrorMessage"] = msg;
                }
            }
            return RedirectToAction(nameof(CreateFromProducto));
        }


        [HttpPost]
        public async Task<IActionResult> Guardar([Bind("ID, Nombre, Estado, RubroID")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                //Check if Nombre exists
                var nom = context.Marcas.FirstOrDefault(x => x.Nombre == marca.Nombre);
                if (nom == null)
                {
                    context.Add(marca);
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

        public IActionResult Edit(int id)
        {
            List<Rubro> rubrosList = context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            Marca editar = context.Marcas.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }
        public IActionResult Detail(int id)
        {
            Marca detail = context.Marcas.Include("Rubro").FirstOrDefault(i => i.ID == id);
            return View(detail);
        }

        public IActionResult Actualizar(int id, string nombre, string estado, int rubroID)
        {
            if (ModelState.IsValid)
            {
                Marca editar = context.Marcas.FirstOrDefault(i => i.ID == id);
                var nom = context.Marcas.FirstOrDefault(x => x.Nombre == nombre);
                if ((nom == null) || (nombre == editar.Nombre) || (nom.RubroID != rubroID))
                {
                    editar.Nombre = nombre;
                    editar.Estado = estado;
                    editar.RubroID = rubroID;
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await context.Marcas.Include("Rubro")
                .FirstOrDefaultAsync(m => m.ID == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await context.Marcas.FindAsync(id);
            context.Marcas.Remove(marca);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> BatchDelete(int[] deleteInputs)
        {
            if (deleteInputs != null && deleteInputs.Length > 0)
            {
                var lista = context.Marcas.Include("Rubro").Where(r => deleteInputs.Contains(r.ID));
                return View(await lista.ToListAsync());
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("BatchConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatchConfirmed(int[] deleteInputs)
        {
            var lista = context.Marcas.Where(r => deleteInputs.Contains(r.ID));
            foreach (var item in lista)
            {
                context.Marcas.Remove(item);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}