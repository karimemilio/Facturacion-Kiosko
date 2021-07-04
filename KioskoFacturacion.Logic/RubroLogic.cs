using System;
using System.Collections.Generic;
using KioskoFacturacion.Web.Models;

namespace KioskoFacturacion.Logic
{
    public class RubroLogic
    {
        List<Rubro> repositorio = new List<Rubro>();
        public void Guardar(string nombre)
        {
            Guardar(new Rubro(nombre));
        }
        public void Guardar(Rubro rubro)
        {
            if (!string.IsNullOrWhiteSpace(rubro.Nombre))
            { this.repositorio.Add(rubro); }
            else { throw new Exception("El nombre no puede estar vacio"); }

        }
        public List<Rubro> Listar()
        {
            return repositorio;
        }
    }
}
