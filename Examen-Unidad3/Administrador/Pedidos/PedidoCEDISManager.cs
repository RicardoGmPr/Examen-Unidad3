using Examen_Unidad3;
using Examen_Unidad3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Examen_Unidad3.Database;

namespace AdminConsoleApp.Utilidades
{
    public class PedidoCEDISManager
    {
        private static readonly List<string> productosNoCEDIS = new List<string>
        {
            "pan kaiser", "lechuga", "tomate", "cebolla", "cebolla morada"
        };

        public static List<Producto> ObtenerProductosCEDIS()
        {
            try
            {
                // Leer directamente de la base de datos
                var todosProductos = InventarioRepository.ObtenerTodos();

                // Filtrar productos que NO son de proveedores externos
                return todosProductos
                    .Where(p => !productosNoCEDIS.Contains(p.Nombre.ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos CEDIS: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Producto>();
            }
        }

        public static void CargarProductosCEDIS(DataGridView dgv)
        {
            try
            {
                dgv.Rows.Clear();
                var productosCEDIS = ObtenerProductosCEDIS();

                int id = 1;
                foreach (var producto in productosCEDIS)
                {
                    string categoria = DeterminarCategoriaProducto(producto.Nombre);
                    dgv.Rows.Add(id,
                                $"{categoria.Substring(0, 4).ToUpper()}-{id:000}",
                                producto.Nombre,
                                categoria,
                                producto.Cantidad,
                                producto.Unidad,
                                0); // Cantidad a pedir (inicial = 0)
                    id++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool ProcesarPedidoCEDIS(DataGridView dgv)
        {
            try
            {
                Inventario inventario = new Inventario();
                inventario.InicializarInventario();
                inventario.CargarInventario("inventario.json");

                var pedidosRealizados = new List<string>();
                bool hayPedidos = false;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[6].Value != null && int.TryParse(row.Cells[6].Value.ToString(), out int cantidadPedir))
                    {
                        if (cantidadPedir > 0)
                        {
                            string nombreProducto = row.Cells[2].Value?.ToString();
                            string unidadProducto = row.Cells[5].Value?.ToString();

                            if (ActualizarProductoEnInventario(inventario, nombreProducto, cantidadPedir))
                            {
                                // ESTA LÍNEA ES CRUCIAL - Asegúrate que esté agregando a la lista
                                pedidosRealizados.Add($"• {cantidadPedir} {unidadProducto} de {nombreProducto}");
                                hayPedidos = true;
                            }
                        }
                    }
                }

                if (hayPedidos)
                {
                    inventario.GuardarInventario("inventario.json");
                    MostrarResumenPedido(pedidosRealizados);  // VERIFICAR QUE ESTA LÍNEA EXISTA
                    return true;
                }
                else
                {
                    MessageBox.Show("No se han especificado cantidades para pedir.", "Información",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar pedido: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static bool ActualizarProductoEnInventario(Inventario inventario, string nombreProducto, int cantidadPedir)
        {
            // Actualizar en la base de datos
            bool actualizado = InventarioRepository.AgregarCantidad(nombreProducto, cantidadPedir, "Pedido CEDIS");

            if (actualizado)
            {
                // También actualizar en el objeto inventario en memoria
                var producto = inventario.congelado.FirstOrDefault(p =>
                    p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (producto != null)
                {
                    producto.Cantidad += cantidadPedir;
                    return true;
                }

                producto = inventario.refrigerado.FirstOrDefault(p =>
                    p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (producto != null)
                {
                    producto.Cantidad += cantidadPedir;
                    return true;
                }

                producto = inventario.secos.FirstOrDefault(p =>
                    p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

                if (producto != null)
                {
                    producto.Cantidad += cantidadPedir;
                    return true;
                }
            }

            return false;
        }

        private static string DeterminarCategoriaProducto(string nombreProducto)
        {
            var productoCongelado = new[] { "Carne", "Papas", "Aros", "Galletas", "Nieve Vainilla", "Nieve Chocolate", "Nieve Fresa" };
            var productoRefrigerado = new[] { "Queso amarillo", "Lechuga", "Tomate", "Cebolla", "Cebolla morada", "Tocino", "Pepinillos" };

            if (productoCongelado.Any(p => p.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)))
                return "Congelado";
            if (productoRefrigerado.Any(p => p.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase)))
                return "Refrigerado";

            return "Seco";
        }

        private static void MostrarResumenPedido(List<string> pedidosRealizados)
        {
            string resumen = "=== PEDIDO CEDIS PROCESADO ===\n\n";
            resumen += $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n";
            resumen += "Productos pedidos:\n";

            foreach (var pedido in pedidosRealizados)
            {
                resumen += pedido + "\n";
            }

            resumen += $"\nTotal de productos: {pedidosRealizados.Count}";
            resumen += "\n\nEl inventario ha sido actualizado y guardado correctamente.";

            MessageBox.Show(resumen, "Pedido CEDIS Completado",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Método para verificar que el archivo se guardó correctamente
        public static void VerificarInventarioActualizado()
        {
            try
            {
                Inventario inventarioVerificacion = new Inventario();
                inventarioVerificacion.CargarInventario("inventario.json");

                MessageBox.Show("Inventario verificado correctamente desde el archivo JSON.", "Verificación",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar inventario: {ex.Message}", "Error de Verificación",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}