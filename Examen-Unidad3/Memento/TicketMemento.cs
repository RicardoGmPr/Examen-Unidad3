using System;
using System.Collections.Generic;

namespace Examen_Unidad3.Memento
{
    public class TicketMemento
    {
        public List<string> Items { get; }
        public decimal Total { get; }
        public int LastTicketNumber { get; }
        public string TipoOrden { get; }
        public string NombreCliente { get; }
        public DateTime FechaCreacion { get; }

        public TicketMemento(List<string> items, decimal total, int lastNumber, string tipoOrden, string nombreCliente)
        {
            Items = new List<string>(items); // Copia profunda
            Total = total;
            LastTicketNumber = lastNumber;
            TipoOrden = tipoOrden;
            NombreCliente = nombreCliente;
            FechaCreacion = DateTime.Now;
        }
    }
}