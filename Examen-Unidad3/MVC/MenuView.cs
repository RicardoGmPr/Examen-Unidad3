using System.Collections.Generic;

namespace Examen_Unidad3.MVC
{
    public interface MenuView
    {
        void ActualizarTicket(List<string> items, decimal total);
        void MostrarMensaje(string mensaje);
    }
}