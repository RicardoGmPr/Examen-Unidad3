using System.Collections.Generic;
using System.Linq;

namespace Examen_Unidad3.MVC
{
    public class MenuModel
    {
        private List<ProductoItem> ticketItems;
        private decimal total;
        private int lastTicketNumber;

        public MenuModel()
        {
            ticketItems = new List<ProductoItem>();
            total = 0;
            lastTicketNumber = 1;
        }

        public ProductoItem ObtenerProducto(string nombre)
        {
            decimal precio = ObtenerPrecioProducto(nombre);
            bool esHamburguesa = new[] { "Clásica", "Famous Star", "Western", "Teriyaki" }.Contains(nombre);

            return new ProductoItem
            {
                Nombre = nombre,
                Precio = precio,
                EsHamburguesa = esHamburguesa
            };
        }

        public void AgregarAlTicket(ProductoItem producto)
        {
            ticketItems.Add(producto);
            total += producto.Precio;
            lastTicketNumber++;
        }

        // ✅ DECORADOR - Agregar ingrediente extra
        public void AgregarIngredienteExtra(string ingrediente, int indiceHamburguesa)
        {
            if (indiceHamburguesa >= 0 && indiceHamburguesa < ticketItems.Count)
            {
                var hamburguesa = ticketItems[indiceHamburguesa];
                if (hamburguesa.EsHamburguesa)
                {
                    decimal precioExtra = ObtenerPrecioIngredienteExtra(ingrediente);

                    // Agregar ingrediente extra como item separado
                    var ingredienteExtra = new ProductoItem
                    {
                        Nombre = $"-- {ingrediente}",
                        Precio = precioExtra,
                        EsIngredienteExtra = true
                    };

                    ticketItems.Insert(indiceHamburguesa + 1, ingredienteExtra);
                    total += precioExtra;
                }
            }
        }

        public void EliminarDelTicket(int indice)
        {
            if (indice >= 0 && indice < ticketItems.Count)
            {
                total -= ticketItems[indice].Precio;
                ticketItems.RemoveAt(indice);
            }
        }

        public List<string> ObtenerTicketItems()
        {
            var items = new List<string>();
            int contador = 1;

            foreach (var item in ticketItems)
            {
                if (item.EsIngredienteExtra)
                {
                    items.Add($"{item.Nombre,-20} ${item.Precio,6:F2}");
                }
                else
                {
                    items.Add($"{contador}: {item.Nombre,-20} ${item.Precio,6:F2}");
                    contador++;
                }
            }
            return items;
        }

        public decimal ObtenerTotal() => total;
        public int ObtenerLastTicketNumber() => lastTicketNumber;

        // ✅ Para sincronizar con Memento
        public void RestaurarEstado(List<string> items, decimal nuevoTotal, int nuevoLastTicketNumber)
        {
            ticketItems.Clear();
            total = nuevoTotal;
            lastTicketNumber = nuevoLastTicketNumber;

            // Reconstruir items desde las líneas del ticket
            // (Implementación simplificada)
        }

        private decimal ObtenerPrecioIngredienteExtra(string ingrediente)
        {
            switch (ingrediente)
            {
                case "Queso": return 8.00m;
                case "Tocino": return 12.00m;
                default: return 5.00m;
            }
        }

        private decimal ObtenerPrecioProducto(string nombre)
        {
            switch (nombre)
            {
                case "Clásica": return 60.00m;
                case "Famous Star": return 75.00m;
                case "Western": return 80.00m;
                case "Teriyaki": return 85.00m;
                case "V Chico": return 15.00m;
                case "V Mediano": return 18.00m;
                case "V Grande": return 21.00m;
                case "F Chicas": return 25.00m;
                case "F Medianas": return 30.00m;
                case "F Grandes": return 35.00m;
                default: return 0.00m;
            }
        }
    }

    public class ProductoItem
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public bool EsHamburguesa { get; set; }
        public bool EsIngredienteExtra { get; set; }
    }
}