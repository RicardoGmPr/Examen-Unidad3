using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Examen_Unidad3
{
    public partial class ComedorLlevar : Form
    {
        private List<string> ticketsAnteriores = new List<string>();
        private int indiceTicketActual = 0;

        public ComedorLlevar()
        {
            InitializeComponent();

            // Aplicar mejoras de diseño
            AgregarHeaderFormulario();
            ConfigurarTextBoxes();
            MejorarPanelTickets();
            MejorarBotonesPrincipales();
            CargarTickets();
            AgregarBotonesNavegacion();
            InicializarAnimaciones();
        }

        private void ConfigurarTextBoxes()
        {
            // Configurar textBox2 (último ticket) - Parte superior
            textBox2.ReadOnly = true;
            textBox2.Font = new Font("Courier New", 9, FontStyle.Regular);
            textBox2.BackColor = Color.FromArgb(40, 40, 40);
            textBox2.ForeColor = Color.LightGreen;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Padding = new Padding(10);

            // Configurar textBox1 (tickets anteriores) - Parte inferior
            textBox1.ReadOnly = true;
            textBox1.Font = new Font("Courier New", 8, FontStyle.Regular);
            textBox1.BackColor = Color.FromArgb(50, 50, 50);
            textBox1.ForeColor = Color.LightBlue;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Padding = new Padding(10);

            // Agregar eventos de teclado para navegación horizontal
            textBox1.KeyDown += TextBox1_KeyDown;
            textBox1.TabStop = true;

            // Agregar separador visual entre textboxes
            AgregarSeparadorVisual();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                // Ir al ticket anterior (más reciente)
                if (indiceTicketActual > 0)
                {
                    indiceTicketActual--;
                    MostrarTicketEnIndice();
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                // Ir al siguiente ticket (más antiguo)
                if (indiceTicketActual < ticketsAnteriores.Count - 1)
                {
                    indiceTicketActual++;
                    MostrarTicketEnIndice();
                }
            }
        }

        private void CargarTickets()
        {
            try
            {
                string carpetaTickets = Path.Combine(Directory.GetCurrentDirectory(), "Tickets");

                if (!Directory.Exists(carpetaTickets))
                {
                    textBox2.Text = "📄 No se encontró la carpeta de tickets.";
                    textBox1.Text = "📄 No hay tickets anteriores disponibles.";
                    return;
                }

                string[] archivosTicket = Directory.GetFiles(carpetaTickets, "Ticket_*.txt");

                if (archivosTicket.Length == 0)
                {
                    textBox2.Text = "📄 No hay tickets disponibles.";
                    textBox1.Text = "📄 No hay tickets anteriores disponibles.";
                    return;
                }

                // Ordenar por fecha de modificación (más recientes primero)
                var archivosOrdenados = archivosTicket
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .ToArray();

                // ÚLTIMO TICKET (textBox2)
                if (archivosOrdenados.Length > 0)
                {
                    CargarUltimoTicket(archivosOrdenados[0]);
                }

                // TICKETS ANTERIORES (textBox1) - Penúltimo + 5 más
                ticketsAnteriores.Clear();
                int ticketsACargar = Math.Min(6, archivosOrdenados.Length - 1); // Máximo 6, excluyendo el último

                for (int i = 1; i <= ticketsACargar; i++) // Empezar desde el índice 1 (penúltimo)
                {
                    string contenido = File.ReadAllText(archivosOrdenados[i], Encoding.UTF8);
                    string ticketFormateado = FormatearTicketParaCarrusel(contenido, archivosOrdenados[i], i);
                    ticketsAnteriores.Add(ticketFormateado);
                }

                // Mostrar el primer ticket anterior (penúltimo)
                indiceTicketActual = 0;
                MostrarTicketEnIndice();
            }
            catch (Exception ex)
            {
                textBox2.Text = $"❌ Error al cargar tickets: {ex.Message}";
                textBox1.Text = "❌ Error al cargar tickets anteriores.";
            }
        }

        private void CargarUltimoTicket(string archivoMasReciente)
        {
            try
            {
                string contenidoTicket = File.ReadAllText(archivoMasReciente, Encoding.UTF8);

                string ticketConHeader = $"🎫 ÚLTIMO TICKET PROCESADO\n";
                ticketConHeader += $"📅 {File.GetLastWriteTime(archivoMasReciente):dd/MM/yyyy HH:mm:ss}\n";
                ticketConHeader += $"📂 {Path.GetFileName(archivoMasReciente)}\n";
                ticketConHeader += new string('=', 50) + "\n\n";
                ticketConHeader += contenidoTicket;

                textBox2.Text = ticketConHeader;
                textBox2.SelectionStart = 0;
                textBox2.ScrollToCaret();
            }
            catch (Exception ex)
            {
                textBox2.Text = $"❌ Error al cargar el último ticket: {ex.Message}";
            }
        }

        private string FormatearTicketParaCarrusel(string contenido, string archivo, int posicion)
        {
            string header = $"🎫 TICKET ANTERIOR #{posicion}\n";
            header += $"📅 {File.GetLastWriteTime(archivo):dd/MM/yyyy HH:mm:ss}\n";
            header += $"📂 {Path.GetFileName(archivo)}\n";
            header += new string('=', 45) + "\n\n";
            return header + contenido;
        }

        private void MostrarTicketEnIndice()
        {
            if (ticketsAnteriores.Count == 0)
            {
                textBox1.Text = "📄 No hay tickets anteriores disponibles.";
                return;
            }

            if (indiceTicketActual >= 0 && indiceTicketActual < ticketsAnteriores.Count)
            {
                string footer = $"\n\n" + new string('━', 50);
                textBox1.Text = ticketsAnteriores[indiceTicketActual] + footer;
                textBox1.SelectionStart = 0;
                textBox1.ScrollToCaret();
            }
        }

        // Método público para actualizar tickets desde otros formularios
        public void ActualizarTickets()
        {
            CargarTickets();
        }

        // Agregar métodos de navegación con botones si quieres
        public void IrTicketAnterior()
        {
            if (indiceTicketActual > 0)
            {
                indiceTicketActual--;
                MostrarTicketEnIndice();
                ActualizarIndicadorPosicion();
            }
        }

        public void IrTicketSiguiente()
        {
            if (indiceTicketActual < ticketsAnteriores.Count - 1)
            {
                indiceTicketActual++;
                MostrarTicketEnIndice();
                ActualizarIndicadorPosicion();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrdenInfo.TipoOrden = "Llevar";
            Teclado tecladoForm = new Teclado();
            tecladoForm.Show();
            this.Hide();
        }

        public static class OrdenInfo
        {
            public static string TipoOrden = "";
            public static string NombreCliente = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OrdenInfo.TipoOrden = "Comedor";
            Teclado tecladoForm = new Teclado();
            this.Close();
            tecladoForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            POSLogin poslogin = new POSLogin();
            poslogin.Show();
            this.Hide();
        }

        // Agregar en InitializeComponent() o en el constructor
        private void AgregarBotonesNavegacion()
        {
            // Botón anterior con estilo moderno
            Button btnAnterior = new Button();
            btnAnterior.Text = "⬅️ Anterior";
            btnAnterior.Size = new Size(200, 60);
            btnAnterior.Location = new Point(800, 1280);
            btnAnterior.BackColor = Color.FromArgb(54, 162, 235);
            btnAnterior.ForeColor = Color.White;
            btnAnterior.FlatStyle = FlatStyle.Flat;
            btnAnterior.FlatAppearance.BorderSize = 0;
            btnAnterior.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnAnterior.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 182, 255);
            btnAnterior.Click += (sender, e) => IrTicketAnterior();
            btnAnterior.BringToFront();
            this.Controls.Add(btnAnterior);

            // Botón siguiente con estilo moderno
            Button btnSiguiente = new Button();
            btnSiguiente.Text = "Siguiente ➡️";
            btnSiguiente.Size = new Size(200, 60);
            btnSiguiente.Location = new Point(1020, 1280);
            btnSiguiente.BackColor = Color.FromArgb(40, 167, 69);
            btnSiguiente.ForeColor = Color.White;
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.FlatAppearance.BorderSize = 0;
            btnSiguiente.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSiguiente.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 187, 89);
            btnSiguiente.Click += (sender, e) => IrTicketSiguiente();
            btnSiguiente.BringToFront();
            this.Controls.Add(btnSiguiente);

            // Indicador de posición
            Label indicadorPosicion = new Label();
            indicadorPosicion.Name = "lblIndicador";
            indicadorPosicion.Text = "📍 Navegando tickets";
            indicadorPosicion.Size = new Size(300, 35);
            indicadorPosicion.Location = new Point(800, 1350);
            indicadorPosicion.BackColor = Color.Transparent;
            indicadorPosicion.ForeColor = Color.FromArgb(255, 204, 0);
            indicadorPosicion.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            indicadorPosicion.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(indicadorPosicion);
            indicadorPosicion.BringToFront();
        }

        //************************************DISEÑO****************************************
        private void MejorarPanelTickets()
        {
            // Panel principal con sombra
            panel1.BackColor = Color.FromArgb(30, 30, 30);
            panel1.BorderStyle = BorderStyle.None;

            // Crear sombra simulada
            Panel sombra = new Panel();
            sombra.BackColor = Color.FromArgb(20, 20, 20);
            sombra.Size = new Size(panel1.Width + 8, panel1.Height + 8);
            sombra.Location = new Point(panel1.Location.X + 4, panel1.Location.Y + 4);
            this.Controls.Add(sombra);
            sombra.SendToBack();

            // Borde redondeado para el panel
            panel1.Region = CrearBordeRedondeado(panel1.Width, panel1.Height, 15);
            sombra.Region = CrearBordeRedondeado(sombra.Width, sombra.Height, 15);
        }

        // Método para crear bordes redondeados
        private Region CrearBordeRedondeado(int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(width - radius, 0, radius, radius, 270, 90);
            path.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            path.AddArc(0, height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return new Region(path);
        }

        

        private void AgregarSeparadorVisual()
        {
            Label separador = new Label();
            separador.Text = "📋 TICKETS ANTERIORES ";
            separador.BackColor = Color.FromArgb(60, 60, 60);
            separador.ForeColor = Color.FromArgb(255, 204, 0);
            separador.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            separador.TextAlign = ContentAlignment.MiddleCenter;
            separador.Size = new Size(772, 35);
            separador.Location = new Point(3, 820);
            panel1.Controls.Add(separador);
            separador.BringToFront();
        }

        private void MejorarBotonesPrincipales()
        {
            // Botón Comedor - Tema restaurante elegante
            button1.BackColor = Color.FromArgb(139, 69, 19); // Marrón elegante
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 82, 45);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(101, 50, 13);
            button1.Font = new Font("Segoe UI", 42, FontStyle.Bold);
            button1.Text = "🍽️ Comedor";

            // Efecto de sombra para button1
            CrearSombraBoton(button1, Color.FromArgb(80, 40, 10));

            // Botón Llevar - Tema delivery moderno
            button2.BackColor = Color.FromArgb(255, 140, 0); // Naranja vibrante
            button2.ForeColor = Color.White;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 165, 0);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 120, 0);
            button2.Font = new Font("Segoe UI", 42, FontStyle.Bold);
            button2.Text = "🥡 Llevar";

            // Efecto de sombra para button2
            CrearSombraBoton(button2, Color.FromArgb(200, 100, 0));

            // Botón Cerrar Sesión - Tema profesional
            button3.BackColor = Color.FromArgb(220, 53, 69);
            button3.ForeColor = Color.White;
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 35, 51);
            button3.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 25, 41);
            button3.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            button3.Text = "🚪 Cerrar Sesión";

            // Traer al frente
            button3.BringToFront();
        }

        private void CrearSombraBoton(Button boton, Color colorSombra)
        {
            Panel sombra = new Panel();
            sombra.BackColor = colorSombra;
            sombra.Size = new Size(boton.Width + 8, boton.Height + 8);
            sombra.Location = new Point(boton.Location.X + 4, boton.Location.Y + 4);
            this.Controls.Add(sombra);
            sombra.SendToBack();
        }

        

        private void AgregarHeaderFormulario()
        {
            // Panel header
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.Width, 80);
            headerPanel.Location = new Point(0, 0);
            headerPanel.BackColor = Color.FromArgb(255, 204, 0);
            headerPanel.Dock = DockStyle.Top;
            this.Controls.Add(headerPanel);

            // Título principal
            Label lblTitulo = new Label();
            lblTitulo.Text = "🍔 CARL'S JR - SELECCIÓN DE SERVICIO";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(45, 45, 48);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(50, 10);
            headerPanel.Controls.Add(lblTitulo);

            // Hora actual
            Label lblHora = new Label();
            lblHora.Name = "lblHoraActual";
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            lblHora.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblHora.ForeColor = Color.FromArgb(45, 45, 48);
            lblHora.Location = new Point(this.Width - 250, 20);
            lblHora.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            headerPanel.Controls.Add(lblHora);

            // Timer para actualizar la hora
            Timer timerHora = new Timer();
            timerHora.Interval = 1000;
            timerHora.Tick += (s, e) => lblHora.Text = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            timerHora.Start();

            // Ajustar posición de otros controles
            AjustarPosicionControles();
        }

        private void AjustarPosicionControles()
        {
            // Mover todos los controles hacia abajo para dar espacio al header
            int offsetY = 90;

            panel1.Location = new Point(panel1.Location.X, panel1.Location.Y + offsetY);
            button1.Location = new Point(button1.Location.X, button1.Location.Y + offsetY);
            button2.Location = new Point(button2.Location.X, button2.Location.Y + offsetY);
            button3.Location = new Point(button3.Location.X, button3.Location.Y + offsetY);
        }


        private void ActualizarIndicadorPosicion()
        {
            Label indicador = this.Controls.Find("lblIndicador", true).FirstOrDefault() as Label;
            if (indicador != null && ticketsAnteriores.Count > 0)
            {
                indicador.Text = $"📍 Ticket {indiceTicketActual + 1} de {ticketsAnteriores.Count}";
            }
        }

        private Timer animacionTimer;

        private void InicializarAnimaciones()
        {
            animacionTimer = new Timer();
            animacionTimer.Interval = 50; // 20 FPS
            animacionTimer.Tick += AnimacionTimer_Tick;
            animacionTimer.Start();
        }

        private void AnimacionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Efecto de respiración simple sin verificar hover
                if (button1 != null && button1.IsHandleCreated)
                {
                    DateTime now = DateTime.Now;
                    double respiracion = Math.Sin(now.Millisecond * 0.005) * 0.05 + 0.95;

                    Color baseColor = Color.FromArgb(139, 69, 19);
                    button1.BackColor = Color.FromArgb(
                        (int)(baseColor.R * respiracion),
                        (int)(baseColor.G * respiracion),
                        (int)(baseColor.B * respiracion)
                    );
                }
            }
            catch
            {
                // Ignorar errores
            }
        }
    }
}