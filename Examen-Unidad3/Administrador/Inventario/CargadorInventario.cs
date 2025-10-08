using Examen_Unidad3;
using Examen_Unidad3.Database;
using System;
using System.Drawing;
using System.Windows.Forms;
using Examen_Unidad3.Database;


namespace AdminConsoleApp.Utilidades
{
    public class CargadorInventario
    {
        public static void CargarDatosInventario(DataGridView dgv)
        {
            try
            {
                // Limpiar el DataGridView
                dgv.Rows.Clear();
                int id = 1;

                // LEER DIRECTAMENTE DE LA BASE DE DATOS
                var productosCongelados = InventarioRepository.ObtenerPorCategoria("Congelado");
                var productosRefrigerados = InventarioRepository.ObtenerPorCategoria("Refrigerado");
                var productosSecos = InventarioRepository.ObtenerPorCategoria("Seco");

                // Cargar productos congelados
                foreach (var producto in productosCongelados)
                {
                    dgv.Rows.Add(id,
                                $"CONG-{id:000}",
                                producto.Nombre,
                                "Congelado",
                                producto.Cantidad,
                                producto.Unidad);
                    id++;
                }

                // Cargar productos refrigerados
                foreach (var producto in productosRefrigerados)
                {
                    dgv.Rows.Add(id,
                                $"REFR-{id:000}",
                                producto.Nombre,
                                "Refrigerado",
                                producto.Cantidad,
                                producto.Unidad);
                    id++;
                }

                // Cargar productos secos
                foreach (var producto in productosSecos)
                {
                    dgv.Rows.Add(id,
                                $"SECO-{id:000}",
                                producto.Nombre,
                                "Seco",
                                producto.Cantidad,
                                producto.Unidad);
                    id++;
                }

                // Aplicar formato visual
                AplicarFormatoVisual(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar inventario: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private static void AplicarFormatoVisual(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                // Aplicar colores por categoría
                AplicarColorCategoria(row);

                // Resaltar productos con cantidad baja
                AplicarColorCantidad(row, dgv);
            }
        }

        private static void AplicarColorCategoria(DataGridViewRow row)
        {
            string categoria = row.Cells[3].Value?.ToString();
            switch (categoria)
            {
                case "Congelado":
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    break;
                case "Refrigerado":
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    break;
                case "Seco":
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                    break;
            }
        }

        private static void AplicarColorCantidad(DataGridViewRow row, DataGridView dgv)
        {
            if (row.Cells[4].Value != null && int.TryParse(row.Cells[4].Value.ToString(), out int cantidad))
            {
                if (cantidad == 0)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
                else if (cantidad <= 5)
                {
                    row.DefaultCellStyle.ForeColor = Color.Orange;
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
            }
        }
    }
}