using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Examen_Unidad3.ComedorLlevar;

namespace Examen_Unidad3
{
    public partial class Teclado : Form
    {
        public Teclado()
        {
            InitializeComponent();
        }

        private void Teclado_Load(object sender, EventArgs e)
        {
            RedondearTodosLosBotones();

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

        private void RedondearTodosLosBotones()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button btn)
                {
                    RedondearButton(btn, 100);
                }
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            Button boton = sender as Button;
            if (boton != null)
            {
                textBox1.Text += boton.Text;
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
        }

        private void button43_Click_1(object sender, EventArgs e)
        {
            textBox1.Text += " ";
        }

        private void button29_Click(object sender, EventArgs e)
        {
            // Supongamos que el TextBox se llama "textBoxEntrada"
            OrdenInfo.NombreCliente = textBox1.Text;

            Menu menuForm = new Menu();
            menuForm.Show();
            this.Hide();
        }
    }
}
