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
        private readonly int _RegistrosPorPagina = 6;
        private Paginacion<Marca> _PaginadorMarcas;
        private ApplicationDbContext context;

        public MarcasController(ApplicationDbContext context)
        {
            this.context = context;

        }
        public async Task<IActionResult> Index(string filter, int pagina = 1)
        {
            int _TotalRegistros = 0;
            // Número total de registros de la tabla Marcas
            _TotalRegistros = context.Marcas.Count();
            ViewData["CurrentFilter"] = filter;
            var marcas = from e in this.context.Marcas
                            .OrderBy(x => x.Nombre)
                            .Skip((pagina - 1) * _RegistrosPorPagina)
                            .Take(_RegistrosPorPagina)
                            .Include("Rubro")
                         select e;

            // Número total de páginas de la tabla Marcas
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores

            if (!String.IsNullOrEmpty(filter))
            {
                marcas = marcas.Where(e => e.Nombre.Contains(filter) || e.Rubro.Nombre.Contains(filter)).OrderBy(x => x.Nombre);
                _TotalRegistros = marcas.Count();
            }
            _PaginadorMarcas = new Paginacion<Marca>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = marcas.ToList()
            };

            return View(_PaginadorMarcas);
        }

        public IActionResult Create()
        {
            List<Rubro> rubrosList = context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Guardar([Bind("ID, Nombre, Estado, RubroID")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                context.Add(marca);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Rubro> rubrosList = context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            return RedirectToAction(nameof(Create));
        }

        public IActionResult Edit(int id)
        {
            List<Rubro> rubrosList = context.Rubros.ToList();
            ViewBag.RubrosList = new SelectList(rubrosList, "ID", "Nombre");
            Marca editar = context.Marcas.FirstOrDefault(i => i.ID == id);
            return View(editar);
        }

        public IActionResult Actualizar(int id, string nombre, string estado, int rubroID)
        {
            Marca editar = context.Marcas.FirstOrDefault(i => i.ID == id);
            editar.Nombre = nombre;
            editar.Estado = estado;
            editar.RubroID = rubroID;
            context.SaveChanges();

            return RedirectToAction("Index");
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