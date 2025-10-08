using Examen_Unidad3.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Examen_Unidad3
{

    // Clase Cajero integrada
    public class Cajero
    {
        public int Id { get; set; }  
        public string Nombre { get; set; }
        public string Clave { get; set; }

        public Cajero() { }

        public Cajero(string nombre, string clave)
        {
            Nombre = nombre;
            Clave = clave;
        }

        public override string ToString()
        {
            return $"{Nombre} - {Clave}";
        }
    }

    public class VerCajerosManager
    {
        // Variables para manejo de cajeros (integradas de CajerosManager)
        private static List<Cajero> cajeros = new List<Cajero>();
        private const string archivoCajeros = "cajeros.txt";

        // Métodos de archivo (integrados de CajerosManager)
        private static void CargarCajerosDesdeArchivo()
        {
            cajeros = CajerosRepository.ObtenerTodos();
        }

        private static void GuardarCajerosEnArchivo()
        {
            //
        }

        private static void AgregarCajero(string nombre, string clave)
        {
            CajerosRepository.Agregar(nombre, clave);
        }

        private static void EliminarCajero(string clave)
        {
            CajerosRepository.Eliminar(clave);
        }

        private static void ModificarClave(string claveActual, string nuevaClave)
        {
            CajerosRepository.ModificarClave(claveActual, nuevaClave);
        }

        private static List<Cajero> ObtenerCajeros()
        {
            return CajerosRepository.ObtenerTodos();
        }

        private static bool ValidarClave(string clave)
        {
            return CajerosRepository.ValidarClave(clave);
        }

        private static string ObtenerNombrePorClave(string clave)
        {
            return CajerosRepository.ObtenerNombrePorClave(clave);
        }

        // Métodos públicos para la UI
        public static void CargarCajeros(DataGridView dgv)
        {
            try
            {
                CargarCajerosDesdeArchivo();

                dgv.Rows.Clear();
                var listaCajeros = ObtenerCajeros();

                if (listaCajeros.Count == 0)
                {
                    dgv.Rows.Add("", "No hay cajeros registrados", "", "");
                    dgv.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                    dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                    return;
                }

                int id = 1;
                foreach (var cajero in listaCajeros)
                {
                    dgv.Rows.Add(id, cajero.Nombre, cajero.Clave, "Activo");

                    if (id % 2 == 0)
                    {
                        dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.AliceBlue;
                    }
                    id++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cajeros: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void FiltrarCajeros(DataGridView dgv, string filtro)
        {
            try
            {
                CargarCajerosDesdeArchivo();

                dgv.Rows.Clear();
                var listaCajeros = ObtenerCajeros();

                int id = 1;
                foreach (var cajero in listaCajeros)
                {
                    if (string.IsNullOrEmpty(filtro) ||
                        cajero.Nombre.ToLower().Contains(filtro.ToLower()))
                    {
                        dgv.Rows.Add(id, cajero.Nombre, cajero.Clave, "Activo");

                        if (id % 2 == 0)
                        {
                            dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.AliceBlue;
                        }
                        id++;
                    }
                }

                if (dgv.Rows.Count == 0)
                {
                    dgv.Rows.Add("", $"No se encontraron cajeros con: '{filtro}'", "", "");
                    dgv.Rows[0].DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar cajeros: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool EliminarCajeroSeleccionado(DataGridView dgv)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un cajero para eliminar.", "Advertencia",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string nombre = dgv.SelectedRows[0].Cells[1].Value?.ToString();
                string clave = dgv.SelectedRows[0].Cells[2].Value?.ToString();

                DialogResult resultado = MessageBox.Show(
                    $"¿Eliminar al cajero '{nombre}' con clave '{clave}'?\n\n⚠️ Esta acción no se puede deshacer.",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    CargarCajerosDesdeArchivo();
                    EliminarCajero(clave);

                    MessageBox.Show($"Cajero '{nombre}' eliminado correctamente.", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cajero: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool AgregarNuevoCajero()
        {
            try
            {
                // Crear formulario simple para agregar cajero
                Form formAgregar = new Form();
                formAgregar.Text = "Agregar Nuevo Cajero";
                formAgregar.Size = new Size(650, 300);
                formAgregar.StartPosition = FormStartPosition.CenterParent;
                formAgregar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formAgregar.MaximizeBox = false;

                // Nombre
                Label lblNombre = new Label();
                lblNombre.Text = "Nombre:";
                lblNombre.Location = new Point(20, 30);
                lblNombre.AutoSize = true;
                formAgregar.Controls.Add(lblNombre);

                TextBox txtNombre = new TextBox();
                txtNombre.Location = new Point(140, 27);
                txtNombre.Size = new Size(200, 20);
                formAgregar.Controls.Add(txtNombre);

                // Clave
                Label lblClave = new Label();
                lblClave.Text = "Clave:";
                lblClave.Location = new Point(20, 65);
                lblClave.AutoSize = true;
                formAgregar.Controls.Add(lblClave);

                TextBox txtClave = new TextBox();
                txtClave.Location = new Point(140, 62);
                txtClave.Size = new Size(200, 20);
                formAgregar.Controls.Add(txtClave);

                // Botones
                Button btnGuardar = new Button();
                btnGuardar.Text = "Guardar";
                btnGuardar.Location = new Point(80, 150);
                btnGuardar.Size = new Size(140, 50);
                btnGuardar.BackColor = Color.LightGreen;
                btnGuardar.DialogResult = DialogResult.OK;
                formAgregar.Controls.Add(btnGuardar);

                Button btnCancelar = new Button();
                btnCancelar.Text = "Cancelar";
                btnCancelar.Location = new Point(270, 150);
                btnCancelar.Size = new Size(140, 50);
                btnCancelar.BackColor = Color.LightGray;
                btnCancelar.DialogResult = DialogResult.Cancel;
                formAgregar.Controls.Add(btnCancelar);

                formAgregar.AcceptButton = btnGuardar;
                formAgregar.CancelButton = btnCancelar;

                if (formAgregar.ShowDialog() == DialogResult.OK)
                {
                    string nombre = txtNombre.Text.Trim();
                    string clave = txtClave.Text.Trim();

                    if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(clave))
                    {
                        MessageBox.Show("Por favor complete todos los campos.", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Validar que no exista la clave
                    CargarCajerosDesdeArchivo();

                    if (ValidarClave(clave))
                    {
                        MessageBox.Show("Ya existe un cajero con esa clave.", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Agregar cajero
                    AgregarCajero(nombre, clave);
                    MessageBox.Show($"Cajero '{nombre}' agregado correctamente.", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cajero: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool ModificarCajeroSeleccionado(DataGridView dgv)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un cajero para modificar.", "Advertencia",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string nombreActual = dgv.SelectedRows[0].Cells[1].Value?.ToString();
                string claveActual = dgv.SelectedRows[0].Cells[2].Value?.ToString();

                // Crear formulario para modificar
                Form formModificar = new Form();
                formModificar.Text = "Modificar Cajero";
                formModificar.Size = new Size(450, 320);
                formModificar.StartPosition = FormStartPosition.CenterParent;
                formModificar.FormBorderStyle = FormBorderStyle.FixedDialog;
                formModificar.MaximizeBox = false;

                // Mostrar datos actuales
                Label lblInfo = new Label();
                lblInfo.Text = $"Modificando: {nombreActual}";
                lblInfo.Location = new Point(20, 20);
                lblInfo.Font = new Font("Arial", 9, FontStyle.Bold);
                lblInfo.AutoSize = true;
                formModificar.Controls.Add(lblInfo);

                // Clave actual
                Label lblClaveActual = new Label();
                lblClaveActual.Text = "Clave actual:";
                lblClaveActual.Location = new Point(20, 70);
                lblClaveActual.AutoSize = true;
                formModificar.Controls.Add(lblClaveActual);

                TextBox txtClaveActual = new TextBox();
                txtClaveActual.Text = claveActual;
                txtClaveActual.Location = new Point(180, 67);
                txtClaveActual.Size = new Size(170, 20);
                txtClaveActual.ReadOnly = true;
                txtClaveActual.BackColor = Color.LightGray;
                formModificar.Controls.Add(txtClaveActual);

                // Nueva clave
                Label lblNuevaClave = new Label();
                lblNuevaClave.Text = "Nueva clave:";
                lblNuevaClave.Location = new Point(20, 105);
                lblNuevaClave.AutoSize = true;
                formModificar.Controls.Add(lblNuevaClave);

                TextBox txtNuevaClave = new TextBox();
                txtNuevaClave.Location = new Point(180, 102);
                txtNuevaClave.Size = new Size(170, 20);
                formModificar.Controls.Add(txtNuevaClave);

                // Botones
                Button btnModificar = new Button();
                btnModificar.Text = "Modificar";
                btnModificar.Location = new Point(20, 160);
                btnModificar.Size = new Size(130, 50);
                btnModificar.BackColor = Color.LightBlue;
                btnModificar.DialogResult = DialogResult.OK;
                formModificar.Controls.Add(btnModificar);

                Button btnCancelar = new Button();
                btnCancelar.Text = "Cancelar";
                btnCancelar.Location = new Point(170, 160);
                btnCancelar.Size = new Size(130, 50);
                btnCancelar.BackColor = Color.LightGray;
                btnCancelar.DialogResult = DialogResult.Cancel;
                formModificar.Controls.Add(btnCancelar);

                formModificar.AcceptButton = btnModificar;
                formModificar.CancelButton = btnCancelar;

                if (formModificar.ShowDialog() == DialogResult.OK)
                {
                    string nuevaClave = txtNuevaClave.Text.Trim();

                    if (string.IsNullOrEmpty(nuevaClave))
                    {
                        MessageBox.Show("La nueva clave no puede estar vacía.", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (nuevaClave == claveActual)
                    {
                        MessageBox.Show("La nueva clave debe ser diferente a la actual.", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Validar que no exista la nueva clave
                    CargarCajerosDesdeArchivo();

                    if (ValidarClave(nuevaClave))
                    {
                        MessageBox.Show("Ya existe un cajero con esa clave.", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Modificar clave
                    ModificarClave(claveActual, nuevaClave);
                    MessageBox.Show($"Clave del cajero '{nombreActual}' modificada correctamente.", "Éxito",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar cajero: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void ExportarLista()
        {
            try
            {
                CargarCajerosDesdeArchivo();
                var listaCajeros = ObtenerCajeros();

                if (listaCajeros.Count == 0)
                {
                    MessageBox.Show("No hay cajeros para exportar.", "Información",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string archivo = $"Lista_Cajeros_{DateTime.Now:yyyyMMdd_HHmm}.txt";
                string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), archivo);

                using (StreamWriter writer = new StreamWriter(ruta))
                {
                    writer.WriteLine("=== LISTA DE CAJEROS ===");
                    writer.WriteLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                    writer.WriteLine($"Total: {listaCajeros.Count} cajeros");
                    writer.WriteLine();

                    int id = 1;
                    foreach (var cajero in listaCajeros)
                    {
                        writer.WriteLine($"{id}. {cajero.Nombre} - Clave: {cajero.Clave}");
                        id++;
                    }
                }

                MessageBox.Show($"Lista exportada: {archivo}", "Éxito",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static int ContarCajeros()
        {
            try
            {
                CargarCajerosDesdeArchivo();
                return ObtenerCajeros().Count;
            }
            catch
            {
                return 0;
            }
        }

        // Métodos públicos adicionales para uso externo (compatibilidad)
        public static bool ValidarClavePublic(string clave)
        {
            CargarCajerosDesdeArchivo();
            return ValidarClave(clave);
        }

        public static string ObtenerNombrePorClavePublic(string clave)
        {
            CargarCajerosDesdeArchivo();
            return ObtenerNombrePorClave(clave);
        }
    }
}