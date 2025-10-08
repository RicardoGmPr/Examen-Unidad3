using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen_Unidad3.Administrador
{
    public partial class LoginAdmin : Form
    {
        public LoginAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "paseo2000" && textBox2.Text.Trim() == "paseo2000")
            {
                MessageBox.Show("¡Acceso concedido!", "Éxito");
                // Crear instancia del formulario destino
                MenuAdmin menuAdmin = new MenuAdmin();
                Inicio inicio = new Inicio();

                // Mostrar el nuevo formulario
                menuAdmin.Show();
                inicio.Show();

                // Cerrar el formulario actual (POSLogin)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Error");
                textBox1.Text = textBox2.Text = ""; // Limpiar campos
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Crear instancia del formulario destino
            Inicio inicio = new Inicio();

            // Mostrar el nuevo formulario
            inicio.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();
        }
    }
}
