using AdminConsoleApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen_Unidad3
{

    public partial class POSLogin : Form
    {
        private VerCajerosManager cajerosManager = new VerCajerosManager();

        public static string claveIngresada = ""; 
        public static string nombre = "";

        public POSLogin()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void POSLogin_Load(object sender, EventArgs e)
        {
            //Redondear bordes de botones
            RedondearButton(button1, 100);
            RedondearButton(button2, 100);
            RedondearButton(button3, 100);
            RedondearButton(button4, 100);
            RedondearButton(button5, 100);
            RedondearButton(button6, 100);
            RedondearButton(button7, 100);
            RedondearButton(button8, 100);
            RedondearButton(button9, 100);
            RedondearButton(button10, 100);
            RedondearButton(buttondlt, 100);
            RedondearButton(buttoningrs, 100);

            //cajerosManager.CargarCajerosDesdeArchivo();

        }

        private void RedondearButton(Button btn, int radio)
        {
            Rectangle rect = btn.ClientRectangle;
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
            path.AddArc(rect.Right - radio, rect.Y, radio, radio, 270, 90);
            path.AddArc(rect.Right - radio, rect.Bottom - radio, radio, radio, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radio, radio, radio, 90, 90);
            path.CloseFigure();

            btn.Region = new Region(path);
        }

        private void buttoningrs_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que la clave no esté vacía
                if (string.IsNullOrWhiteSpace(claveIngresada))
                {
                    MessageBox.Show("Por favor ingrese una clave.", "Campo Vacío",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar clave usando nuestra clase
                if (VerCajerosManager.ValidarClavePublic(claveIngresada))
                {
                    nombre = VerCajerosManager.ObtenerNombrePorClavePublic(claveIngresada);

                    MessageBox.Show($"¡Bienvenido, {nombre}!", "Acceso Concedido",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir el nuevo formulario
                    ComedorLlevar nuevoFormulario = new ComedorLlevar();
                    nuevoFormulario.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Clave incorrecta. Verifique e intente nuevamente.", "Acceso Denegado",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Limpiar clave
                    claveIngresada = "";

                    // Si tienes controles visuales, limpiarlos también
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar credenciales: {ex.Message}", "Error del Sistema",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Limpiar en caso de error
                claveIngresada = "";

            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                claveIngresada += btn.Text;
            }
        }

        private void buttondlt_Click(object sender, EventArgs e)
        {
            if (claveIngresada.Length > 0)
            {
                claveIngresada = claveIngresada.Substring(0, claveIngresada.Length - 1);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();

            // Ahora que ComedorLlevar se cerró, podemos cerrar el formulario de Venta
            this.Hide();
        }

    }
}
