using Examen_Unidad3.Database;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Examen_Unidad3
{
    public class ExportadorInventario
    {
        public static void ExportarListaReabastecimiento(Inventario inventario, int stockMinimo)
        {
            try
            {
                // Obtener productos directamente de BD
                var todosProductos = InventarioRepository.ObtenerTodos();
                var productosReabastecer = todosProductos
                    .Where(p => p.Cantidad <= stockMinimo)
                    .ToList();

                if (productosReabastecer.Count == 0)
                {
                    MessageBox.Show("No hay productos para exportar con el filtro actual.", "Información",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Crear nombre del archivo con fecha y hora
                string nombreArchivo = $"Lista_Reabastecimiento_{DateTime.Now:yyyyMMdd_HHmm}.txt";
                string rutaCompleta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);

                // Crear el contenido del archivo
                using (StreamWriter writer = new StreamWriter(rutaCompleta, false, Encoding.UTF8))
                {
                    // Encabezado del archivo
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine("                    LISTA DE REABASTECIMIENTO                    ");
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine($"Generada el: {DateTime.Now:dddd, dd 'de' MMMM 'de' yyyy 'a las' HH:mm:ss}");
                    writer.WriteLine($"Filtro aplicado: Stock mínimo ≤ {stockMinimo}");
                    writer.WriteLine($"Total de productos: {productosReabastecer.Count}");
                    writer.WriteLine();

                    // Separación por urgencia
                    var productosUrgentes = productosReabastecer.Where(p => p.Cantidad == 0).ToList();
                    var productosProximos = productosReabastecer.Where(p => p.Cantidad > 0 && p.Cantidad <= 5).ToList();
                    var productosNormales = productosReabastecer.Where(p => p.Cantidad > 5).ToList();

                    // PRODUCTOS URGENTES (Stock = 0)
                    if (productosUrgentes.Count > 0)
                    {
                        writer.WriteLine("🚨 PRODUCTOS URGENTES (STOCK = 0) 🚨");
                        writer.WriteLine("───────────────────────────────────────────────────────────────");

                        foreach (var producto in productosUrgentes)
                        {
                            string categoria = ObtenerCategoria(producto);
                            int stockSugerido = 20;
                            int cantidadNecesaria = stockSugerido - producto.Cantidad;

                            writer.WriteLine($"• {producto.Nombre}");
                            writer.WriteLine($"  Categoría: {categoria}");
                            writer.WriteLine($"  Stock actual: {producto.Cantidad} {producto.Unidad}");
                            writer.WriteLine($"  Stock sugerido: {stockSugerido} {producto.Unidad}");
                            writer.WriteLine($"  Cantidad necesaria: {cantidadNecesaria} {producto.Unidad}");
                            writer.WriteLine($"  Prioridad: ⭐⭐⭐ ALTA");
                            writer.WriteLine();
                        }
                    }

                    // PRODUCTOS PRÓXIMOS A TERMINARSE (Stock 1-5)
                    if (productosProximos.Count > 0)
                    {
                        writer.WriteLine("⚠️  PRODUCTOS PRÓXIMOS A TERMINARSE (STOCK 1-5) ⚠️");
                        writer.WriteLine("───────────────────────────────────────────────────────────────");

                        foreach (var producto in productosProximos)
                        {
                            string categoria = ObtenerCategoria(producto);
                            int stockSugerido = 20;
                            int cantidadNecesaria = stockSugerido - producto.Cantidad;

                            writer.WriteLine($"• {producto.Nombre}");
                            writer.WriteLine($"  Categoría: {categoria}");
                            writer.WriteLine($"  Stock actual: {producto.Cantidad} {producto.Unidad}");
                            writer.WriteLine($"  Stock sugerido: {stockSugerido} {producto.Unidad}");
                            writer.WriteLine($"  Cantidad necesaria: {cantidadNecesaria} {producto.Unidad}");
                            writer.WriteLine($"  Prioridad: ⭐⭐ MEDIA");
                            writer.WriteLine();
                        }
                    }

                    // PRODUCTOS CON STOCK MEDIO (Stock 6+)
                    if (productosNormales.Count > 0)
                    {
                        writer.WriteLine("📋 PRODUCTOS CON STOCK MEDIO (STOCK 6+)");
                        writer.WriteLine("───────────────────────────────────────────────────────────────");

                        foreach (var producto in productosNormales)
                        {
                            string categoria = ObtenerCategoria(producto);
                            int stockSugerido = 20;
                            int cantidadNecesaria = stockSugerido - producto.Cantidad;

                            writer.WriteLine($"• {producto.Nombre}");
                            writer.WriteLine($"  Categoría: {categoria}");
                            writer.WriteLine($"  Stock actual: {producto.Cantidad} {producto.Unidad}");
                            writer.WriteLine($"  Stock sugerido: {stockSugerido} {producto.Unidad}");
                            writer.WriteLine($"  Cantidad necesaria: {cantidadNecesaria} {producto.Unidad}");
                            writer.WriteLine($"  Prioridad: ⭐ BAJA");
                            writer.WriteLine();
                        }
                    }

                    // RESUMEN FINAL
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine("                           RESUMEN                             ");
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine($"Total de productos a reabastecer: {productosReabastecer.Count}");
                    writer.WriteLine($"• Productos urgentes (stock = 0): {productosUrgentes.Count}");
                    writer.WriteLine($"• Productos próximos a terminarse: {productosProximos.Count}");
                    writer.WriteLine($"• Productos con stock medio: {productosNormales.Count}");
                    writer.WriteLine();

                    // Separación por categorías
                    var congeladosCount = productosReabastecer.Count(p => ObtenerCategoria(p) == "Congelado");
                    var refrigeradosCount = productosReabastecer.Count(p => ObtenerCategoria(p) == "Refrigerado");
                    var secosCount = productosReabastecer.Count(p => ObtenerCategoria(p) == "Seco");

                    writer.WriteLine("DISTRIBUCIÓN POR CATEGORÍAS:");
                    writer.WriteLine($"• Productos congelados: {congeladosCount}");
                    writer.WriteLine($"• Productos refrigerados: {refrigeradosCount}");
                    writer.WriteLine($"• Productos secos: {secosCount}");
                    writer.WriteLine();

                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine($"Archivo generado por: Sistema de Inventario");
                    writer.WriteLine($"Fecha de generación: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                }

                // Mostrar mensaje de éxito con opción de abrir el archivo
                DialogResult resultado = MessageBox.Show(
                    $"Lista exportada correctamente:\n\n{rutaCompleta}\n\n¿Desea abrir el archivo ahora?",
                    "Exportación Exitosa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (resultado == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("notepad.exe", rutaCompleta);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("No se tiene permisos para escribir en la ubicación seleccionada.\nIntente ejecutar el programa como administrador.",
                               "Error de Permisos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("No se pudo encontrar la carpeta de destino.",
                               "Error de Directorio", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error de entrada/salida al crear el archivo:\n{ex.Message}",
                               "Error de E/S", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado al exportar:\n{ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ObtenerCategoria(Producto producto)  // Quitar el parámetro inventario
        {
            var productoCongelado = new[] { "Carne", "Papas", "Aros", "Galletas", "Nieve Vainilla", "Nieve Chocolate", "Nieve Fresa", "Galleta" };
            var productoRefrigerado = new[] { "Queso amarillo", "Lechuga", "Tomate", "Cebolla", "Cebolla morada", "Tocino", "Pepinillos" };

            if (productoCongelado.Any(p => p.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase)))
                return "Congelado";
            if (productoRefrigerado.Any(p => p.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase)))
                return "Refrigerado";

            return "Seco";
        }
    }
}