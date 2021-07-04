using System;
using System.Collections.Generic;
using KioskoFacturacion.Logic;
using KioskoFacturacion.Web.Models;

namespace KioskoFacturacion.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            RubroLogic logic = new RubroLogic();
            logic.Guardar("Perfumeria");
            // logic.Guardar("");
            //logic.Guardar(new Rubro("Comestibles"));
            // logic.Guardar(new Rubro(""));

            List<Rubro> list = logic.Listar();
            foreach (var item in list)
            { Console.WriteLine(item.Nombre); }

        }
    }
}
