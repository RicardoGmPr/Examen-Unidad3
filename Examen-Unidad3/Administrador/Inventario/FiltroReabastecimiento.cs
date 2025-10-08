using Examen_Unidad3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Examen_Unidad3
{
    public class FiltroReabastecimiento
    {
        public static void FiltrarArticulosReabastecer(DataGridView dgvArticulos, Label lblUrgentes, Label lblProximos, int stockMinimo)
        {
            try
            {
                if (dgvArticulos == null) return;

                // Obtener productos directamente de la base de datos
                var productosCongelados = InventarioRepository.ObtenerPorCategoria("Congelado");
                var productosRefrigerados = InventarioRepository.ObtenerPorCategoria("Refrigerado");
                var productosSecos = InventarioRepository.ObtenerPorCategoria("Seco");

                // Limpia el DataGridView
                dgvArticulos.Rows.Clear();

                // Obtener productos que necesitan reabastecimiento
                var productosUrgentes = productosCongelados
                .Concat(productosRefrigerados)
                .Concat(productosSecos)
                .Where(p => p.Cantidad <= stockMinimo)
                .ToList();

                if (productosUrgentes.Count == 0)
                {
                    MostrarMensajeSinProductos(dgvArticulos);
                    ActualizarContadores(lblUrgentes, lblProximos, 0, 0);
                    return;
                }

                // Procesar productos
                var contadores = ProcesarProductos(dgvArticulos, productosUrgentes);

                // Actualizar contadores
                ActualizarContadores(lblUrgentes, lblProximos, contadores.urgentes, contadores.proximos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar artículos: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static (int urgentes, int proximos) ProcesarProductos(DataGridView dgvArticulos, List<Producto> productosUrgentes)
        {
            int id = 1;
            int contadorUrgentes = 0;
            int contadorProximos = 0;

            foreach (var producto in productosUrgentes)
            {
                // Determinar la categoría
                string categoria = DeterminarCategoriaDesdeNombre(producto.Nombre);

                // Calcular valores
                int stockSugerido = 20;
                int cantidadNecesaria = stockSugerido - producto.Cantidad;

                // Agregar fila
                dgvArticulos.Rows.Add(id,
                                     $"{categoria.Substring(0, 4).ToUpper()}-{id:000}",
                                     producto.Nombre,
                                     categoria,
                                     producto.Cantidad,
                                     stockSugerido,
                                     cantidadNecesaria);

                // Aplicar formato y contar
                var ultimaFila = dgvArticulos.Rows[dgvArticulos.Rows.Count - 1];
                AplicarFormatoUrgencia(ultimaFila, producto, dgvArticulos, ref contadorUrgentes, ref contadorProximos);

                id++;
            }

            return (contadorUrgentes, contadorProximos);
        }

        private static void AplicarFormatoUrgencia(DataGridViewRow row, Producto producto, DataGridView dgv, ref int contadorUrgentes, ref int contadorProximos)
        {
            if (producto.Cantidad == 0)
            {
                row.DefaultCellStyle.BackColor = Color.LightCoral;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
                row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                contadorUrgentes++;
            }
            else if (producto.Cantidad <= 5)
            {
                row.DefaultCellStyle.BackColor = Color.LightYellow;
                row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                contadorProximos++;
            }
            else
            {
                // Aplicar color por categoría para productos con stock medio
                string categoria = row.Cells[3].Value?.ToString();
                AplicarColorCategoria(row, categoria);
            }
        }

        private static void AplicarColorCategoria(DataGridViewRow row, string categoria)
        {
            switch (categoria)
            {
                case "Congelado":
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    break;
                case "Refrigerado":
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    break;
                case "Seco":
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    break;
            }
        }

        private static void MostrarMensajeSinProductos(DataGridView dgvArticulos)
        {
            dgvArticulos.Rows.Add("", "", "No hay productos que necesiten reabastecimiento", "", "", "", "");
            dgvArticulos.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvArticulos.Rows[0].DefaultCellStyle.Font = new Font(dgvArticulos.Font, FontStyle.Italic);
        }

        private static void ActualizarContadores(Label lblUrgentes, Label lblProximos, int contadorUrgentes, int contadorProximos)
        {
            if (lblUrgentes != null)
                lblUrgentes.Text = $"Productos urgentes (stock = 0): {contadorUrgentes}";

            if (lblProximos != null)
                lblProximos.Text = $"Productos próximos a terminarse (stock 1-5): {contadorProximos}";
        }

        private static string DeterminarCategoriaDesdeNombre(string nombreProducto)
        {
            var productoCongelado = new[] { "Carne", "Papas", "Aros", "Galletas", "Nieve Vainilla", "Nieve Chocolate", "Nieve Fresa" };
            var productoRefrigerado = new[] { "Queso amarillo", "Lechuga", "Tomate", "Cebolla", "Cebolla morada", "Tocino", "Pepinillos" };

            if (productoCongelado.Any(p => p.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)))
                return "Congelado";
            if (productoRefrigerado.Any(p => p.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)))
                return "Refrigerado";

            return "Seco";
        }
    }
}
