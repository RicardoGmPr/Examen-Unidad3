using Examen_Unidad3.Decorador;
using System.Collections.Generic;

namespace Examen_Unidad3.Prototype
{
    public class PrototypeRegistry
    {
        private Dictionary<string, HamburguesaPrototype> prototipos;

        public PrototypeRegistry()
        {
            prototipos = new Dictionary<string, HamburguesaPrototype>();
            InicializarPrototipos();
        }

        private void InicializarPrototipos()
        {
            var clasica = new HamburguesaPrototype("Clásica", 60.00m);
            clasica.Ingredientes.AddRange(new[] { "Pan", "Carne", "Lechuga" });
            prototipos["Clásica"] = clasica;

            var famous = new HamburguesaPrototype("Famous Star", 75.00m);
            famous.Ingredientes.AddRange(new[] { "Pan", "Carne", "Queso", "Lechuga" });
            prototipos["Famous Star"] = famous;
        }

        public Hamburguesa ObtenerHamburguesa(string tipo)
        {
            if (prototipos.ContainsKey(tipo))
                return prototipos[tipo].Clonar(); // ✅ AQUÍ SE USA EL PATRÓN

            return null;
        }
    }
}