using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3.Decorador
{
    // Decoradores concretos
    public class QuesoExtra : HamburguesaDecorador
    {
        public QuesoExtra(Hamburguesa hamburguesa) : base(hamburguesa) { }

        public override decimal ObtenerPrecio()
        {
            return base.ObtenerPrecio() + 10.00m; // Precio adicional del queso
        }
    }
}
