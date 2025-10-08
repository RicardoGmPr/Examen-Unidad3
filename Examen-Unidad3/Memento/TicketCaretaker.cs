using System.Collections.Generic;
using System.Linq;

namespace Examen_Unidad3.Memento
{
    public class TicketCaretaker
    {
        private Stack<TicketMemento> historialTickets;
        private const int MAX_HISTORIAL = 10; // Máximo 10 estados guardados

        public TicketCaretaker()
        {
            historialTickets = new Stack<TicketMemento>();
        }

        public void GuardarEstado(TicketMemento memento)
        {
            // Limitar el historial para no usar demasiada memoria
            if (historialTickets.Count >= MAX_HISTORIAL)
            {
                var tempList = historialTickets.ToList();
                tempList.RemoveAt(tempList.Count - 1); // Remover el más antiguo
                historialTickets = new Stack<TicketMemento>(tempList.AsEnumerable().Reverse());
            }

            historialTickets.Push(memento);
        }

        public TicketMemento RestaurarUltimoEstado()
        {
            if (historialTickets.Count > 0)
            {
                return historialTickets.Pop();
            }
            return null;
        }

        public bool TieneEstadosGuardados()
        {
            return historialTickets.Count > 0;
        }

        public int CantidadEstadosGuardados()
        {
            return historialTickets.Count;
        }
    }
}