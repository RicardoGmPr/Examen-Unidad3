using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3.Decorador
{
    //Implementacion concreta base
    public class HamburguesaBase : Hamburguesa
    {
        public HamburguesaBase(string nombre)
        {
            Nombre = nombre;
        }

        public override decimal ObtenerPrecio()
        {
            switch (Nombre)
            {
                case "Clásica": return 60.00m;
                case "Famous Star": return 75.00m;
                case "Western": return 80.00m;
                case "Teriyaki": return 85.00m;
                default: return 0.00m;
            }
        }
    }
}
