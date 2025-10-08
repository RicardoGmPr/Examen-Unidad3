using System.Collections.Generic;
using Examen_Unidad3.Decorador;

namespace Examen_Unidad3.Prototype
{
    public class HamburguesaPrototype : Hamburguesa, IPrototype<Hamburguesa>
    {
        public override string Nombre { get; set; }
        public decimal Precio { get; set; }
        public List<string> Ingredientes { get; set; }

        public HamburguesaPrototype(string nombre, decimal precio)
        {
            Nombre = nombre;
            Precio = precio;
            Ingredientes = new List<string>();
        }

        public Hamburguesa Clonar()
        {
            var clone = new HamburguesaPrototype(this.Nombre, this.Precio);
            clone.Ingredientes = new List<string>(this.Ingredientes);
            return clone;
        }

        public override decimal ObtenerPrecio() => Precio;
    }
}