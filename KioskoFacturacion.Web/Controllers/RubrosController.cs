using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using KioskoFacturacion.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Data;
using EFCore.BulkExtensions;

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
        public async Task<IActionResult> Index(string filter)
        {
            ViewData["CurrentFilter"] = filter;
            var rubros = from e in this.context.Rubros
                         select e;

            if (!String.IsNullOrEmpty(filter))
            {
                rubros = rubros.Where(e => e.Nombre.Contains(filter));
            }

            return View(await rubros.ToListAsync());
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

