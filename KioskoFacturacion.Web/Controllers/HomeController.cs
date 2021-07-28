using Microsoft.AspNetCore.Mvc;
using KioskoFacturacion.Web.Data;
using Microsoft.AspNetCore.Authorization;

namespace KioskoFacturacion.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Gestion()
        {
            return View();
        }
    }
}