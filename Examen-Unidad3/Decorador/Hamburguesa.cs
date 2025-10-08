using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3.Decorador
{
    //Interfaz - Clase base
    public abstract class Hamburguesa
    {
        public virtual string Nombre { get; set; }
        public abstract decimal ObtenerPrecio();
    }
}
