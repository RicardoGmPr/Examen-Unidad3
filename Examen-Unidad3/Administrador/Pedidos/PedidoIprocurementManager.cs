using Examen_Unidad3;
using Examen_Unidad3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Examen_Unidad3
{
    public class PedidoIprocurementManager
    {
        // Productos específicos para cada proveedor
        private static readonly List<string> productosBimbo = new List<string> { "pan kaiser" };
        private static readonly List<string> productosVerdura = new List<string>
        {
            "lechuga", "tomate", "cebolla", "cebolla morada"
        };

        public static List<Producto> ObtenerProductosBimbo()
        {
            try
            {
                var todosProductos = InventarioRepository.ObtenerTodos();

                return todosProductos
                    .Where(p => productosBimbo.Any(pb => p.Nombre.ToLower().Contains(pb)))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos Bimbo: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Producto>();
            }
        }

        public static List<Producto> ObtenerProductosVerdura()
        {
            try
            {
                var todosProductos = InventarioRepository.ObtenerTodos();

                return todosProductos
                    .Where(p => productosVerdura.Any(pv => p.Nombre.ToLower().Contains(pv)))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos de verdura: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Producto>();
            }
        }

        public static void CargarProductosBimbo(DataGridView dgv)
        {
            try
            {
                dgv.Rows.Clear();
                var productosBimboList = ObtenerProductosBimbo();

                int id = 1;
                foreach (var producto in productosBimboList)
                {
                    dgv.Rows.Add(id,
                                $"BIMBO-{id:000}",
                                producto.Nombre,
                                "Seco",
                                producto.Cantidad,
                                producto.Unidad,
                                0); // Cantidad a pedir
                    id++;
                }

                if (productosBimboList.Count == 0)
                {
                    dgv.Rows.Add("", "", "No hay productos Bimbo disponibles", "", "", "", "");
                    dgv.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                    dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos Bimbo: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CargarProductosVerdura(DataGridView dgv)
        {
            try
            {
                dgv.Rows.Clear();
                var productosVerduraList = ObtenerProductosVerdura();

                int id = 1;
                foreach (var producto in productosVerduraList)
                {
                    dgv.Rows.Add(id,
                                $"VERD-{id:000}",
                                producto.Nombre,
                                "Refrigerado",
                                producto.Cantidad,
                                producto.Unidad,
                                0); // Cantidad a pedir
                    id++;
                }

                if (productosVerduraList.Count == 0)
                {
                    dgv.Rows.Add("", "", "No hay productos de verdura disponibles", "", "", "", "");
                    dgv.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                    dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos de verdura: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool ProcesarPedidoBimbo(DataGridView dgv)
        {
            return ProcesarPedidoGenerico(dgv, "Bimbo S.A de C.V", "secos");
        }

        public static bool ProcesarPedidoVerdura(DataGridView dgv)
        {
            return ProcesarPedidoGenerico(dgv, "Market And Cash De Aguascalientes S.A de C.V", "refrigerado");
        }

        private static bool ProcesarPedidoGenerico(DataGridView dgv, string proveedor, string categoria)
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

                            if (ActualizarProductoEnInventario(inventario, nombreProducto, cantidadPedir, categoria))
                            {
                                pedidosRealizados.Add($"• {cantidadPedir} {unidadProducto} de {nombreProducto}");
                                hayPedidos = true;
                            }
                        }
                    }
                }

                if (hayPedidos)
                {
                    inventario.GuardarInventario("inventario.json");
                    MostrarResumenPedido(pedidosRealizados, proveedor);
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

        private static bool ActualizarProductoEnInventario(Inventario inventario, string nombreProducto, int cantidadPedir, string categoria)
        {
            List<Producto> listaProductos = categoria switch
            {
                "secos" => inventario.secos,
                "refrigerado" => inventario.refrigerado,
                "congelado" => inventario.congelado,
                _ => new List<Producto>()
            };

            var producto = listaProductos.FirstOrDefault(p =>
                p.Nombre.Equals(nombreProducto, StringComparison.OrdinalIgnoreCase));

            if (producto != null)
            {
                producto.Cantidad += cantidadPedir;
                return true;
            }

            return false;
        }

        private static void MostrarResumenPedido(List<string> pedidosRealizados, string proveedor)
        {
            string resumen = $"=== PEDIDO {proveedor.ToUpper()} PROCESADO ===\n\n";
            resumen += $"Proveedor: {proveedor}\n";
            resumen += $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n";
            resumen += "Productos pedidos:\n";

            foreach (var pedido in pedidosRealizados)
            {
                resumen += pedido + "\n";
            }

            resumen += $"\nTotal de productos: {pedidosRealizados.Count}";
            resumen += "\n\nEl inventario ha sido actualizado y guardado correctamente.";

            MessageBox.Show(resumen, "Pedido Iprocurement Completado",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}