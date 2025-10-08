using Examen_Unidad3.Administrador;
using Microsoft.Data.Sqlite;

namespace Examen_Unidad3
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Crear instancia del formulario destino
            POSLogin poslogin = new POSLogin();

            // Mostrar el nuevo formulario
            poslogin.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();


            /*
            // Crear instancia del formulario destino
            Teclado teclado = new Teclado();

            // Mostrar el nuevo formulario
            teclado.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();
            */
            /*
            // Crear instancia del formulario destino
            ComedorLlevar comedorllevar = new ComedorLlevar();

            // Mostrar el nuevo formulario
            comedorllevar.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();
            */
            /*
            // Crear instancia del formulario destino
            Menu menu = new Menu();

            // Mostrar el nuevo formulario
            menu.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();
            */
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timerFechaHora_Tick(object sender, EventArgs e)
        {
            lblFechaHora.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm:ss");
        }

        private void lblFechaHora_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Crear instancia del formulario destino
            LoginAdmin loginAdmin = new LoginAdmin();

            // Mostrar el nuevo formulario
            loginAdmin.Show();

            // Cerrar el formulario actual (POSLogin)
            this.Hide();
        }
    }
}
