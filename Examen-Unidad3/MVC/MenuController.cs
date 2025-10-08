using System.Collections.Generic;
using System.Windows.Forms;
using Examen_Unidad3.Prototype;
using Examen_Unidad3.Memento;
using Examen_Unidad3.Decorador;

namespace Examen_Unidad3.MVC
{
    public class MenuController
    {
        private MenuModel model;
        private MenuView view;

        // ✅ PATRONES INTEGRADOS:
        private PrototypeRegistry prototypeRegistry;
        private TicketCaretaker ticketCaretaker;
        private TicketOriginator ticketOriginator;
        private ListBox listBoxTicket;

        public MenuController(MenuView view, ListBox listBox)
        {
            this.view = view;
            this.model = new MenuModel();
            this.listBoxTicket = listBox;

            // ✅ INICIALIZAR PATRONES:
            this.prototypeRegistry = new PrototypeRegistry();
            this.ticketCaretaker = new TicketCaretaker();
            this.ticketOriginator = new TicketOriginator();
        }

        public void AgregarProducto(string nombreProducto, decimal total, int lastTicketNumber)
        {
            // ✅ 1. MEMENTO - Guardar estado antes del cambio
            var memento = ticketOriginator.CrearMemento(listBoxTicket, total, lastTicketNumber);
            ticketCaretaker.GuardarEstado(memento);

            // ✅ 2. PROTOTYPE - Intentar usar prototipo
            var hamburguesaPrototype = prototypeRegistry.ObtenerHamburguesa(nombreProducto);
            if (hamburguesaPrototype != null)
            {
                // Usar prototipo encontrado
                var producto = new ProductoItem
                {
                    Nombre = nombreProducto,
                    Precio = hamburguesaPrototype.ObtenerPrecio(),
                    EsHamburguesa = true
                };
                model.AgregarAlTicket(producto);
                return;
            }

            // Si no hay prototipo, usar precio normal
            var productoNormal = model.ObtenerProducto(nombreProducto);
            if (productoNormal != null)
            {
                model.AgregarAlTicket(productoNormal);
            }
        }

        // ✅ 3. DECORADOR - Agregar ingredientes extra
        public void AgregarIngredienteExtra(string ingrediente, int indiceHamburguesa, decimal total, int lastTicketNumber)
        {
            // Memento antes del cambio
            var memento = ticketOriginator.CrearMemento(listBoxTicket, total, lastTicketNumber);
            ticketCaretaker.GuardarEstado(memento);

            // Usar decorador
            model.AgregarIngredienteExtra(ingrediente, indiceHamburguesa);
        }

        public void EliminarProducto(int indice, decimal total, int lastTicketNumber)
        {
            // Memento antes de eliminar
            var memento = ticketOriginator.CrearMemento(listBoxTicket, total, lastTicketNumber);
            ticketCaretaker.GuardarEstado(memento);

            model.EliminarDelTicket(indice);
        }

        // ✅ 4. MEMENTO - Deshacer
        public bool DeshacerUltimaAccion()
        {
            var estadoAnterior = ticketCaretaker.RestaurarUltimoEstado();
            if (estadoAnterior != null)
            {
                decimal total;
                int lastTicketNumber;
                ticketOriginator.RestaurarDesdeMemento(estadoAnterior, listBoxTicket, out total, out lastTicketNumber);
                return true;
            }
            return false;
        }
    }
}