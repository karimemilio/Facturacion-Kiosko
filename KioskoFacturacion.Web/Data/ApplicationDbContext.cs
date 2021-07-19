using System;
using System.Collections.Generic;
using System.Text;
using KioskoFacturacion.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KioskoFacturacion.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<IdentityUser> Usuarios { get; set; }
        public DbSet<Rubro> Rubros { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
