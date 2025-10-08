using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3.Decorador
{
    //Decoradores concretos
    public class BaconExtra : HamburguesaDecorador
    {
        public BaconExtra(Hamburguesa hamburguesa) : base(hamburguesa) { }

        public override decimal ObtenerPrecio()
        {
            return base.ObtenerPrecio() + 15.00m; // Precio adicional del bacon
        }
    }
}
