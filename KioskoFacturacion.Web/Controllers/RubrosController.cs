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
        private readonly int _RegistrosPorPagina = 6;
        private Paginacion<Rubro> _PaginadorRubros;

        private ApplicationDbContext context;

        public RubrosController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(string filter, int pagina = 1)
        {
            int _TotalRegistros = 0;
            // Número total de registros de la tabla Rubros
            _TotalRegistros = context.Rubros.Count();
            ViewData["CurrentFilter"] = filter;
            var rubros = from e in this.context.Rubros
                            .OrderBy(x => x.Nombre)
                            .Skip((pagina - 1) * _RegistrosPorPagina)
                            .Take(_RegistrosPorPagina)
                         select e;

            // Número total de páginas de la tabla Rubros
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores

            if (!String.IsNullOrEmpty(filter))
            {
                rubros = rubros.Where(e => e.Nombre.Contains(filter)).OrderBy(x => x.Nombre);
                _TotalRegistros = rubros.Count();
            }
            _PaginadorRubros = new Paginacion<Rubro>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = rubros.ToList()
            };

            return View(_PaginadorRubros);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([Bind("Nombre, Estado")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                context.Add(rubro);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }

        public IActionResult Edit(int id)
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

