using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3.Decorador
{
    public abstract class HamburguesaDecorador : Hamburguesa
    {
        protected Hamburguesa hamburguesa;

        public HamburguesaDecorador(Hamburguesa hamburguesa)
        {
            this.hamburguesa = hamburguesa;
        }

        public override string Nombre => hamburguesa.Nombre;

        public override decimal ObtenerPrecio()
        {
            return hamburguesa.ObtenerPrecio();
        }
    }
}
