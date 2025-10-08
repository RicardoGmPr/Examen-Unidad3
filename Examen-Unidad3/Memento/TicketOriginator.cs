using System.Collections.Generic;
using System.Windows.Forms;
using static Examen_Unidad3.ComedorLlevar;

namespace Examen_Unidad3.Memento
{
    public class TicketOriginator
    {
        // Crear memento con el estado actual
        public TicketMemento CrearMemento(ListBox listBoxTicket, decimal total, int lastTicketNumber)
        {
            var items = new List<string>();
            foreach (var item in listBoxTicket.Items)
            {
                items.Add(item.ToString());
            }

            return new TicketMemento(
                items,
                total,
                lastTicketNumber,
                OrdenInfo.TipoOrden,
                OrdenInfo.NombreCliente
            );
        }

        // Restaurar estado desde memento
        public void RestaurarDesdeMemento(TicketMemento memento, ListBox listBoxTicket, out decimal total, out int lastTicketNumber)
        {
            // Limpiar ticket actual
            listBoxTicket.Items.Clear();

            // Restaurar items
            foreach (var item in memento.Items)
            {
                listBoxTicket.Items.Add(item);
            }

            // Restaurar valores
            total = memento.Total;
            lastTicketNumber = memento.LastTicketNumber;

            // Restaurar info de orden
            OrdenInfo.TipoOrden = memento.TipoOrden;
            OrdenInfo.NombreCliente = memento.NombreCliente;
        }
    }
}