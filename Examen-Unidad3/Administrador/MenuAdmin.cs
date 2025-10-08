using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdminConsoleApp;
using AdminConsoleApp.Utilidades; // Asegúrate de agregar esta referencia


namespace Examen_Unidad3.Administrador
{
    public partial class MenuAdmin : Form
    {
        // Declara un panel para cada sección de tu menú
        private Panel panelContenedor;
        private Panel panelVerInventario;
        private Panel panelArticulosReabastecer;
        private Panel panelPedidoCEDIS;
        private Panel panelPedidoIprocurement;
        private Panel panelVerCajeros;
        private Panel panelCorteCaja;

        // Constructor del formulario
        public MenuAdmin()
        {
            InitializeComponent();
            ConfigurarPaneles();        // Primero crear los paneles

        }


        private void ConfigurarPaneles()
        {
            // Panel contenedor principal - asegurar que esté DESPUÉS del MenuStrip
            panelContenedor = new Panel();
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.BackColor = Color.White;
            this.Controls.Add(panelContenedor);
            panelContenedor.BringToFront(); // Asegurar que esté al frente

            // Configurar cada panel individual

            // Panel Ver Inventario
            panelVerInventario = new Panel();
            panelVerInventario.Dock = DockStyle.Fill;
            panelVerInventario.BackColor = Color.White;
            ConfigurarPanelVerInventario(); 
            panelContenedor.Controls.Add(panelVerInventario);

            // Panel Artículos a Reabastecer
            panelArticulosReabastecer = new Panel();
            panelArticulosReabastecer.Dock = DockStyle.Fill;
            panelArticulosReabastecer.BackColor = Color.White;
            ConfigurarPanelArticulosReabastecer();
            panelContenedor.Controls.Add(panelArticulosReabastecer);

            // Panel Pedido CEDIS
            panelPedidoCEDIS = new Panel();
            panelPedidoCEDIS.Dock = DockStyle.Fill;
            panelPedidoCEDIS.BackColor = Color.White;
            ConfigurarPanelPedidoCEDIS();
            panelContenedor.Controls.Add(panelPedidoCEDIS);

            // Panel Pedido Iprocurement
            panelPedidoIprocurement = new Panel();
            panelPedidoIprocurement.Dock = DockStyle.Fill;
            panelPedidoIprocurement.BackColor = Color.White;
            ConfigurarPanelPedidoIprocurement();
            panelContenedor.Controls.Add(panelPedidoIprocurement);

            // Panel Ver Cajeros
            panelVerCajeros = new Panel();
            panelVerCajeros.Dock = DockStyle.Fill;
            panelVerCajeros.BackColor = Color.White;
            ConfigurarPanelVerCajeros();
            panelContenedor.Controls.Add(panelVerCajeros);

            // Panel Corte de Caja
            panelCorteCaja = new Panel();
            panelCorteCaja.Dock = DockStyle.Fill;
            panelCorteCaja.BackColor = Color.White;
            ConfigurarPanelCorteCaja();
            panelContenedor.Controls.Add(panelCorteCaja);

            // Ocultar todos los paneles al inicio
            OcultarTodosPaneles();

            // Mostrar el panel inicial (puedes elegir cualquiera)
            panelVerInventario.Visible = true;
        }

        private void OcultarTodosPaneles()
        {
            panelVerInventario.Visible = false;
            panelArticulosReabastecer.Visible = false;
            panelPedidoCEDIS.Visible = false;
            panelPedidoIprocurement.Visible = false;
            panelVerCajeros.Visible = false;
            panelCorteCaja.Visible = false;
        }

        private void ConfigurarPanelVerInventario()
        {

            // Título del panel
            Label lblTitulo = new Label();
            lblTitulo.Text = "Inventario Completo";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(10, 10);
            panelVerInventario.Controls.Add(lblTitulo);

            // DataGridView
            DataGridView dgvInventario = new DataGridView();
            dgvInventario.Name = "dgvInventario";
            dgvInventario.Margin = new Padding(10);
            dgvInventario.Location = new Point(20, 60); 
            dgvInventario.Size = new Size(1040, 500);    // Tamaño fijo
            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.AllowUserToDeleteRows = false;
            dgvInventario.ReadOnly = true;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas CORRECTAMENTE
            dgvInventario.ColumnCount = 6;
            dgvInventario.Columns[0].Name = "ID";           // Índice 0
            dgvInventario.Columns[1].Name = "Código";       // Índice 1
            dgvInventario.Columns[2].Name = "Producto";     // Índice 2
            dgvInventario.Columns[3].Name = "Categoría";    // Índice 3 - ASEGÚRATE DE QUE ESTÉ AQUÍ
            dgvInventario.Columns[4].Name = "Cantidad";     // Índice 4
            dgvInventario.Columns[5].Name = "Unidad";       // Índice 5

            // Ajustar anchos de columnas
            dgvInventario.Columns[0].Width = 50;
            dgvInventario.Columns[1].Width = 200;
            dgvInventario.Columns[2].Width = 250;
            dgvInventario.Columns[3].Width = 160;
            dgvInventario.Columns[4].Width = 130;


            // Agregar controles al panel
            panelVerInventario.Controls.Add(dgvInventario);

            // Cargar datos iniciales
            CargarDatosInventario(dgvInventario);
        }

        private void ConfigurarPanelArticulosReabastecer()
        {
            // Título del panel
            Label lblTitulo = new Label();
            lblTitulo.Text = "Artículos a Reabastecer";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelArticulosReabastecer.Controls.Add(lblTitulo);

            // Filtros
            Label lblFiltroMinimo = new Label();
            lblFiltroMinimo.Text = "Stock mínimo:";
            lblFiltroMinimo.Location = new Point(20, 70);
            lblFiltroMinimo.AutoSize = true;
            panelArticulosReabastecer.Controls.Add(lblFiltroMinimo);

            NumericUpDown nudStockMinimo = new NumericUpDown();
            nudStockMinimo.Name = "nudStockMinimo";
            nudStockMinimo.Location = new Point(210, 70);
            nudStockMinimo.Size = new Size(80, 20);
            nudStockMinimo.Minimum = 0;
            nudStockMinimo.Maximum = 50;
            nudStockMinimo.Value = 5; // Por defecto mostrar productos con 5 o menos
            panelArticulosReabastecer.Controls.Add(nudStockMinimo);

            Button btnFiltrar = new Button();
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.Location = new Point(300, 68);
            btnFiltrar.Size = new Size(90, 45);
            btnFiltrar.Click += (sender, e) => FiltrarArticulosReabastecer((int)nudStockMinimo.Value);
            panelArticulosReabastecer.Controls.Add(btnFiltrar);

            // DataGridView para mostrar artículos - SIN DOCK
            DataGridView dgvArticulos = new DataGridView();
            dgvArticulos.Name = "dgvArticulosReabastecer";
            dgvArticulos.Location = new Point(20, 130);  // Debajo de los filtros
            dgvArticulos.Size = new Size(1040, 350);     // Tamaño fijo
            dgvArticulos.AllowUserToAddRows = false;
            dgvArticulos.AllowUserToDeleteRows = false;
            dgvArticulos.ReadOnly = true;
            dgvArticulos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArticulos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas específicas para reabastecimiento
            dgvArticulos.ColumnCount = 7;
            dgvArticulos.Columns[0].Name = "ID";
            dgvArticulos.Columns[1].Name = "Código";
            dgvArticulos.Columns[2].Name = "Producto";
            dgvArticulos.Columns[3].Name = "Categoría";
            dgvArticulos.Columns[4].Name = "Actual";
            dgvArticulos.Columns[5].Name = "Sugerido";
            dgvArticulos.Columns[6].Name = "Necesario";

            // Ajustar anchos
            dgvArticulos.Columns[0].Width = 60;   // ID
            dgvArticulos.Columns[1].Width = 200;   // Código
            dgvArticulos.Columns[2].Width = 240;  // Producto
            dgvArticulos.Columns[3].Width = 180;   // Categoría
            dgvArticulos.Columns[4].Width = 120;   // Stock Actual
            dgvArticulos.Columns[5].Width = 150;   // Stock Sugerido
            dgvArticulos.Columns[6].Width = 150;  // Cantidad Necesaria

            panelArticulosReabastecer.Controls.Add(dgvArticulos);

            // Etiquetas de información
            Label lblUrgentes = new Label();
            lblUrgentes.Name = "lblUrgentes";
            lblUrgentes.Text = "Productos urgentes (stock = 0): 0";
            lblUrgentes.Location = new Point(20, 490);
            lblUrgentes.AutoSize = true;
            lblUrgentes.ForeColor = Color.Red;
            lblUrgentes.Font = new Font("Arial", 9, FontStyle.Bold);
            panelArticulosReabastecer.Controls.Add(lblUrgentes);

            Label lblProximos = new Label();
            lblProximos.Name = "lblProximos";
            lblProximos.Text = "Productos próximos a terminarse: 0";
            lblProximos.Location = new Point(300, 490);
            lblProximos.AutoSize = true;
            lblProximos.ForeColor = Color.Orange;
            lblProximos.Font = new Font("Arial", 9, FontStyle.Bold);
            panelArticulosReabastecer.Controls.Add(lblProximos);

            Button btnExportar = new Button();
            btnExportar.Text = "Exportar Lista";
            btnExportar.Location = new Point(50, 530);
            btnExportar.Size = new Size(120, 40);
            btnExportar.Click += (sender, e) => ExportarListaReabastecimiento();
            panelArticulosReabastecer.Controls.Add(btnExportar);

            // Cargar datos iniciales
            FiltrarArticulosReabastecer(5);
        }


        private void ConfigurarPanelPedidoCEDIS()
        {
            // Título del panel
            Label lblTitulo = new Label();
            lblTitulo.Text = "Pedido CEDIS";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPedidoCEDIS.Controls.Add(lblTitulo);

            // DataGridView para mostrar productos CEDIS
            DataGridView dgvPedidoCEDIS = new DataGridView();
            dgvPedidoCEDIS.Name = "dgvPedidoCEDIS";
            dgvPedidoCEDIS.Location = new Point(20, 80);
            dgvPedidoCEDIS.Size = new Size(1040, 500);
            dgvPedidoCEDIS.AllowUserToAddRows = false;
            dgvPedidoCEDIS.AllowUserToDeleteRows = false;
            dgvPedidoCEDIS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidoCEDIS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas
            dgvPedidoCEDIS.ColumnCount = 7;
            dgvPedidoCEDIS.Columns[0].Name = "ID";
            dgvPedidoCEDIS.Columns[1].Name = "Código";
            dgvPedidoCEDIS.Columns[2].Name = "Producto";
            dgvPedidoCEDIS.Columns[3].Name = "Categoría";
            dgvPedidoCEDIS.Columns[4].Name = "Stock Actual";
            dgvPedidoCEDIS.Columns[5].Name = "Unidad";
            dgvPedidoCEDIS.Columns[6].Name = "Pedido";

            // Ajustar anchos
            dgvPedidoCEDIS.Columns[0].Width = 20;   // ID
            dgvPedidoCEDIS.Columns[1].Width = 50;   // Código
            dgvPedidoCEDIS.Columns[2].Width = 80;  // Producto
            dgvPedidoCEDIS.Columns[3].Width = 60;   // Categoría
            dgvPedidoCEDIS.Columns[4].Width = 30;   // Stock Actual
            dgvPedidoCEDIS.Columns[5].Width = 30;   // Unidad
            dgvPedidoCEDIS.Columns[6].Width = 100;  // Cantidad a Pedir

            // Hacer editable solo la columna de "Pedido"
            for (int i = 0; i < 6; i++)
            {
                dgvPedidoCEDIS.Columns[i].ReadOnly = true;
            }
            dgvPedidoCEDIS.Columns[6].ReadOnly = false; // Solo esta columna es editable

            panelPedidoCEDIS.Controls.Add(dgvPedidoCEDIS);

            // Instrucciones
            Label lblInstrucciones = new Label();
            lblInstrucciones.Text = "💡 Instrucciones: Haga doble clic en la columna 'Pedido' para especificar cuánto desea pedir de cada producto.";
            lblInstrucciones.Location = new Point(20, 590);
            lblInstrucciones.Size = new Size(700, 30);
            lblInstrucciones.ForeColor = Color.Gray;
            lblInstrucciones.Font = new Font("Arial", 8, FontStyle.Italic);
            panelPedidoCEDIS.Controls.Add(lblInstrucciones);

            // Botones de acción
            Button btnCargarProductos = new Button();
            btnCargarProductos.Text = "Cargar Productos";
            btnCargarProductos.Location = new Point(20, 620);
            btnCargarProductos.Size = new Size(120, 40);
            btnCargarProductos.Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de que desea procesar este pedido?\nEsto actualizará el inventario permanentemente.",
                    "Confirmar Pedido",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (PedidoCEDISManager.ProcesarPedidoCEDIS(dgvPedidoCEDIS))
                    {
                        // Recargar productos para mostrar el inventario actualizado
                        PedidoCEDISManager.CargarProductosCEDIS(dgvPedidoCEDIS);

                        // Mostrar mensaje adicional de confirmación
                        MessageBox.Show("¡Pedido procesado exitosamente!\nEl archivo inventario.json ha sido actualizado.",
                                       "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            };
            panelPedidoCEDIS.Controls.Add(btnCargarProductos);

            Button btnLimpiarPedido = new Button();
            btnLimpiarPedido.Text = "Limpiar Pedido";
            btnLimpiarPedido.Location = new Point(150, 420);
            btnLimpiarPedido.Size = new Size(120, 30);
            btnLimpiarPedido.Click += (sender, e) =>
            {
                foreach (DataGridViewRow row in dgvPedidoCEDIS.Rows)
                {
                    row.Cells[6].Value = 0; // Resetear cantidad a pedir
                }
                MessageBox.Show("Pedido limpiado.", "Información",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            panelPedidoCEDIS.Controls.Add(btnLimpiarPedido);

            Button btnProcesarPedido = new Button();
            btnProcesarPedido.Text = "Procesar Pedido";
            btnProcesarPedido.Location = new Point(280, 420);
            btnProcesarPedido.Size = new Size(120, 30);
            btnProcesarPedido.BackColor = Color.Green;
            btnProcesarPedido.ForeColor = Color.White;
            btnProcesarPedido.Font = new Font("Arial", 9, FontStyle.Bold);
            btnProcesarPedido.Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de que desea procesar este pedido?\nEsto actualizará el inventario.",
                    "Confirmar Pedido",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (PedidoCEDISManager.ProcesarPedidoCEDIS(dgvPedidoCEDIS))
                    {
                        // Recargar productos para mostrar el inventario actualizado
                        PedidoCEDISManager.CargarProductosCEDIS(dgvPedidoCEDIS);
                    }
                }
            };
            panelPedidoCEDIS.Controls.Add(btnProcesarPedido);

            // Cargar productos iniciales
            PedidoCEDISManager.CargarProductosCEDIS(dgvPedidoCEDIS);
        }

        private void ConfigurarPanelPedidoIprocurement()
        {
            // Título del panel
            Label lblTitulo = new Label();
            lblTitulo.Text = "Pedido Iprocurement";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 20);
            panelPedidoIprocurement.Controls.Add(lblTitulo);

            // Información del sistema
            Label lblInfo = new Label();
            lblInfo.Text = "Sistema de pedidos para proveedores Iprocurement";
            lblInfo.Location = new Point(20, 70);
            lblInfo.Size = new Size(500, 40);
            lblInfo.ForeColor = Color.Blue;
            panelPedidoIprocurement.Controls.Add(lblInfo);

            // Tabs para separar Bimbo y Verduras
            TabControl tabControl = new TabControl();
            tabControl.Name = "tabControlIprocurement";
            tabControl.Location = new Point(20, 120);
            tabControl.Size = new Size(1040, 660);

            // Tab para Bimbo
            TabPage tabBimbo = new TabPage("Bimbo S.A de C.V");
            ConfigurarTabBimbo(tabBimbo);
            tabControl.TabPages.Add(tabBimbo);

            // Tab para Verduras
            TabPage tabVerdura = new TabPage("Market And Cash De Aguascalientes");
            ConfigurarTabVerdura(tabVerdura);
            tabControl.TabPages.Add(tabVerdura);

            panelPedidoIprocurement.Controls.Add(tabControl);

            // Botones generales
            Button btnActualizar = new Button();
            btnActualizar.Text = "🔄 Actualizar Todo";
            btnActualizar.Location = new Point(20, 450);
            btnActualizar.Size = new Size(120, 30);
            btnActualizar.BackColor = Color.LightBlue;
            btnActualizar.Click += (sender, e) => ActualizarTabsIprocurement();
            panelPedidoIprocurement.Controls.Add(btnActualizar);
        }

        private void ConfigurarTabBimbo(TabPage tabBimbo)
        {
            // Información del proveedor
            Label lblProveedorBimbo = new Label();
            lblProveedorBimbo.Text = "Proveedor: Bimbo S.A de C.V - Productos de panadería";
            lblProveedorBimbo.Location = new Point(10, 10);
            lblProveedorBimbo.Size = new Size(400, 30);
            lblProveedorBimbo.Font = new Font("Arial", 9, FontStyle.Bold);
            lblProveedorBimbo.ForeColor = Color.DarkBlue;
            tabBimbo.Controls.Add(lblProveedorBimbo);

            // DataGridView para productos Bimbo
            DataGridView dgvBimbo = new DataGridView();
            dgvBimbo.Name = "dgvBimbo";
            dgvBimbo.Location = new Point(10, 40);
            dgvBimbo.Size = new Size(1040, 220);
            dgvBimbo.AllowUserToAddRows = false;
            dgvBimbo.AllowUserToDeleteRows = false;
            dgvBimbo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBimbo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas
            dgvBimbo.ColumnCount = 7;
            dgvBimbo.Columns[0].Name = "ID";
            dgvBimbo.Columns[1].Name = "Código";
            dgvBimbo.Columns[2].Name = "Producto";
            dgvBimbo.Columns[3].Name = "Categoría";
            dgvBimbo.Columns[4].Name = "Stock Actual";
            dgvBimbo.Columns[5].Name = "Unidad";
            dgvBimbo.Columns[6].Name = "Cantidad a Pedir";

            // Hacer editable solo la última columna
            for (int i = 0; i < 6; i++)
            {
                dgvBimbo.Columns[i].ReadOnly = true;
            }
            dgvBimbo.Columns[6].ReadOnly = false;

            tabBimbo.Controls.Add(dgvBimbo);

            Button btnLimpiarBimbo = new Button();
            btnLimpiarBimbo.Text = "Limpiar Pedido";
            btnLimpiarBimbo.Location = new Point(140, 300);
            btnLimpiarBimbo.Size = new Size(120, 45);
            btnLimpiarBimbo.Click += (sender, e) =>
            {
                foreach (DataGridViewRow row in dgvBimbo.Rows)
                {
                    row.Cells[6].Value = 0;
                }
            };
            tabBimbo.Controls.Add(btnLimpiarBimbo);

            Button btnProcesarBimbo = new Button();
            btnProcesarBimbo.Text = "Procesar Pedido Bimbo";
            btnProcesarBimbo.Location = new Point(270, 300);
            btnProcesarBimbo.Size = new Size(150, 45);
            btnProcesarBimbo.BackColor = Color.Orange;
            btnProcesarBimbo.ForeColor = Color.White;
            btnProcesarBimbo.Font = new Font("Arial", 9, FontStyle.Bold);
            btnProcesarBimbo.Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de procesar el pedido a Bimbo S.A de C.V?",
                    "Confirmar Pedido Bimbo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (PedidoIprocurementManager.ProcesarPedidoBimbo(dgvBimbo))
                    {
                        PedidoIprocurementManager.CargarProductosBimbo(dgvBimbo);
                    }
                }
            };
            tabBimbo.Controls.Add(btnProcesarBimbo);

            // Cargar productos iniciales
            PedidoIprocurementManager.CargarProductosBimbo(dgvBimbo);
        }

        private void ConfigurarTabVerdura(TabPage tabVerdura)
        {
            // Información del proveedor
            Label lblProveedorVerdura = new Label();
            lblProveedorVerdura.Text = "Proveedor: Market And Cash De Aguascalientes S.A de C.V - Productos frescos";
            lblProveedorVerdura.Location = new Point(10, 10);
            lblProveedorVerdura.Size = new Size(700, 30);
            lblProveedorVerdura.Font = new Font("Arial", 9, FontStyle.Bold);
            lblProveedorVerdura.ForeColor = Color.DarkGreen;
            tabVerdura.Controls.Add(lblProveedorVerdura);

            // DataGridView para productos de verdura
            DataGridView dgvVerdura = new DataGridView();
            dgvVerdura.Name = "dgvVerdura";
            dgvVerdura.Location = new Point(10, 40);
            dgvVerdura.Size = new Size(1040, 220);
            dgvVerdura.AllowUserToAddRows = false;
            dgvVerdura.AllowUserToDeleteRows = false;
            dgvVerdura.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVerdura.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas
            dgvVerdura.ColumnCount = 7;
            dgvVerdura.Columns[0].Name = "ID";
            dgvVerdura.Columns[1].Name = "Código";
            dgvVerdura.Columns[2].Name = "Producto";
            dgvVerdura.Columns[3].Name = "Categoría";
            dgvVerdura.Columns[4].Name = "Stock Actual";
            dgvVerdura.Columns[5].Name = "Unidad";
            dgvVerdura.Columns[6].Name = "Cantidad a Pedir";

            // Hacer editable solo la última columna
            for (int i = 0; i < 6; i++)
            {
                dgvVerdura.Columns[i].ReadOnly = true;
            }
            dgvVerdura.Columns[6].ReadOnly = false;

            tabVerdura.Controls.Add(dgvVerdura);

            Button btnLimpiarVerdura = new Button();
            btnLimpiarVerdura.Text = "Limpiar Pedido";
            btnLimpiarVerdura.Location = new Point(140, 300);
            btnLimpiarVerdura.Size = new Size(120, 45);
            btnLimpiarVerdura.Click += (sender, e) =>
            {
                foreach (DataGridViewRow row in dgvVerdura.Rows)
                {
                    row.Cells[6].Value = 0;
                }
            };
            tabVerdura.Controls.Add(btnLimpiarVerdura);

            Button btnProcesarVerdura = new Button();
            btnProcesarVerdura.Text = "Procesar Pedido Verdura";
            btnProcesarVerdura.Location = new Point(270, 300);
            btnProcesarVerdura.Size = new Size(150, 45);
            btnProcesarVerdura.BackColor = Color.Green;
            btnProcesarVerdura.ForeColor = Color.White;
            btnProcesarVerdura.Font = new Font("Arial", 9, FontStyle.Bold);
            btnProcesarVerdura.Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de procesar el pedido a Market And Cash?",
                    "Confirmar Pedido Verdura",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (PedidoIprocurementManager.ProcesarPedidoVerdura(dgvVerdura))
                    {
                        PedidoIprocurementManager.CargarProductosVerdura(dgvVerdura);
                    }
                }
            };
            tabVerdura.Controls.Add(btnProcesarVerdura);

            // Cargar productos iniciales
            PedidoIprocurementManager.CargarProductosVerdura(dgvVerdura);
        }

        private void ActualizarTabsIprocurement()
        {
            TabControl tabControl = panelPedidoIprocurement.Controls.Find("tabControlIprocurement", true).FirstOrDefault() as TabControl;

            if (tabControl != null)
            {
                // Actualizar tab Bimbo
                DataGridView dgvBimbo = tabControl.TabPages[0].Controls.Find("dgvBimbo", true).FirstOrDefault() as DataGridView;
                if (dgvBimbo != null)
                {
                    PedidoIprocurementManager.CargarProductosBimbo(dgvBimbo);
                }

                // Actualizar tab Verdura
                DataGridView dgvVerdura = tabControl.TabPages[1].Controls.Find("dgvVerdura", true).FirstOrDefault() as DataGridView;
                if (dgvVerdura != null)
                {
                    PedidoIprocurementManager.CargarProductosVerdura(dgvVerdura);
                }
            }
        }


        private void ConfigurarPanelVerCajeros()
        {
            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Gestión de Cajeros";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.AutoSize = true;
            panelVerCajeros.Controls.Add(lblTitulo);

            // Buscador
            Label lblBuscar = new Label();
            lblBuscar.Text = "Buscar:";
            lblBuscar.Location = new Point(20, 75);
            lblBuscar.AutoSize = true;
            panelVerCajeros.Controls.Add(lblBuscar);

            TextBox txtBuscar = new TextBox();
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Location = new Point(120, 73);
            txtBuscar.Size = new Size(200, 20);
            txtBuscar.TextChanged += (sender, e) =>
            {
                DataGridView dgv = panelVerCajeros.Controls.Find("dgvCajeros", true)[0] as DataGridView;
                VerCajerosManager.FiltrarCajeros(dgv, txtBuscar.Text);
            };
            panelVerCajeros.Controls.Add(txtBuscar);

            // Contador
            Label lblContador = new Label();
            lblContador.Name = "lblContador";
            lblContador.Text = $"Total: {VerCajerosManager.ContarCajeros()} cajeros";
            lblContador.Location = new Point(350, 75);
            lblContador.AutoSize = true;
            lblContador.ForeColor = Color.Blue;
            lblContador.Font = new Font("Arial", 9, FontStyle.Bold);
            panelVerCajeros.Controls.Add(lblContador);

            // Tabla
            DataGridView dgvCajeros = new DataGridView();
            dgvCajeros.Name = "dgvCajeros";
            dgvCajeros.Location = new Point(20, 125);
            dgvCajeros.Size = new Size(600, 280);
            dgvCajeros.AllowUserToAddRows = false;
            dgvCajeros.ReadOnly = true;
            dgvCajeros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCajeros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCajeros.ColumnCount = 4;
            dgvCajeros.Columns[0].Name = "ID";
            dgvCajeros.Columns[1].Name = "Nombre";
            dgvCajeros.Columns[2].Name = "Clave";
            dgvCajeros.Columns[3].Name = "Estado";

            panelVerCajeros.Controls.Add(dgvCajeros);

            // Botones
            Button btnAgregar = new Button();
            btnAgregar.Text = "➕ Agregar";
            btnAgregar.Location = new Point(20, 455);
            btnAgregar.Size = new Size(100, 50);
            btnAgregar.BackColor = Color.LightGreen;
            btnAgregar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnAgregar.Click += (sender, e) =>
            {
                if (VerCajerosManager.AgregarNuevoCajero())
                {
                    VerCajerosManager.CargarCajeros(dgvCajeros);
                    ActualizarContador();
                }
            };
            panelVerCajeros.Controls.Add(btnAgregar);

            Button btnModificar = new Button();
            btnModificar.Text = "✏️ Modificar";
            btnModificar.Location = new Point(130, 455);
            btnModificar.Size = new Size(100, 50);
            btnModificar.BackColor = Color.LightBlue;
            btnModificar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnModificar.Click += (sender, e) =>
            {
                if (VerCajerosManager.ModificarCajeroSeleccionado(dgvCajeros))
                {
                    VerCajerosManager.CargarCajeros(dgvCajeros);
                }
            };
            panelVerCajeros.Controls.Add(btnModificar);

            Button btnEliminar = new Button();
            btnEliminar.Text = "🗑️ Eliminar";
            btnEliminar.Location = new Point(240, 455);
            btnEliminar.Size = new Size(100, 50);
            btnEliminar.BackColor = Color.LightCoral;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Font = new Font("Arial", 9, FontStyle.Bold);
            btnEliminar.Click += (sender, e) =>
            {
                if (VerCajerosManager.EliminarCajeroSeleccionado(dgvCajeros))
                {
                    VerCajerosManager.CargarCajeros(dgvCajeros);
                    ActualizarContador();
                }
            };
            panelVerCajeros.Controls.Add(btnEliminar);

            Button btnExportar = new Button();
            btnExportar.Text = "📋 Exportar";
            btnExportar.Location = new Point(350, 455);
            btnExportar.Size = new Size(100, 50);
            btnExportar.BackColor = Color.LightYellow;
            btnExportar.Click += (sender, e) => VerCajerosManager.ExportarLista();
            panelVerCajeros.Controls.Add(btnExportar);

            // Método auxiliar para actualizar contador
            void ActualizarContador()
            {
                Label lbl = panelVerCajeros.Controls.Find("lblContador", true)[0] as Label;
                lbl.Text = $"Total: {VerCajerosManager.ContarCajeros()} cajeros";
            }

            // Cargar datos iniciales
            VerCajerosManager.CargarCajeros(dgvCajeros);
        }






        private void ConfigurarPanelCorteCaja()
        {
            // Título del panel (más abajo)
            Label lblTitulo = new Label();
            lblTitulo.Text = "Corte de Caja";
            lblTitulo.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(20, 25);
            panelCorteCaja.Controls.Add(lblTitulo);

            // Información de estado (más abajo y separado)
            Label lblEstado = new Label();
            lblEstado.Name = "lblEstadoTickets";
            lblEstado.Text = CorteCajaManager.ExistenTickets()
                ? $"📄 {CorteCajaManager.ContarTickets()} tickets encontrados"
                : "⚠️ No se encontraron tickets";
            lblEstado.Location = new Point(300, 35);
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = CorteCajaManager.ExistenTickets() ? Color.Green : Color.Red;
            lblEstado.Font = new Font("Arial", 9, FontStyle.Bold);
            panelCorteCaja.Controls.Add(lblEstado);


            // Resumen de totales (MÁS A LA DERECHA y MÁS ABAJO)
            GroupBox gbResumen = new GroupBox();
            gbResumen.Text = "Resumen General";
            gbResumen.Location = new Point(700, 25);
            gbResumen.Size = new Size(320, 160);

            Label lblTotalVentas = new Label();
            lblTotalVentas.Name = "lblTotalVentas";
            lblTotalVentas.Text = "Total Ventas: $0.00";
            lblTotalVentas.Location = new Point(15, 35);
            lblTotalVentas.AutoSize = true;
            lblTotalVentas.Font = new Font("Arial", 9, FontStyle.Bold);
            lblTotalVentas.ForeColor = Color.DarkGreen;
            gbResumen.Controls.Add(lblTotalVentas);

            Label lblTotalPropinas = new Label();
            lblTotalPropinas.Name = "lblTotalPropinas";
            lblTotalPropinas.Text = "Total Propinas: $0.00";
            lblTotalPropinas.Location = new Point(15, 65);
            lblTotalPropinas.AutoSize = true;
            lblTotalPropinas.Font = new Font("Arial", 9, FontStyle.Bold);
            lblTotalPropinas.ForeColor = Color.DarkBlue;
            gbResumen.Controls.Add(lblTotalPropinas);

            Label lblTotalGeneral = new Label();
            lblTotalGeneral.Name = "lblTotalGeneral";
            lblTotalGeneral.Text = "Total General: $0.00";
            lblTotalGeneral.Location = new Point(15, 95);
            lblTotalGeneral.AutoSize = true;
            lblTotalGeneral.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTotalGeneral.ForeColor = Color.DarkRed;
            gbResumen.Controls.Add(lblTotalGeneral);

            Label lblTotalTickets = new Label();
            lblTotalTickets.Name = "lblTotalTickets";
            lblTotalTickets.Text = "Total Tickets: 0";
            lblTotalTickets.Location = new Point(19, 130);
            lblTotalTickets.AutoSize = true;
            lblTotalTickets.Font = new Font("Arial", 9);
            lblTotalTickets.ForeColor = Color.Gray;
            gbResumen.Controls.Add(lblTotalTickets);

            panelCorteCaja.Controls.Add(gbResumen);

            // DataGridView EXTENDIDO A LA DERECHA y MÁS ABAJO
            DataGridView dgvReporte = new DataGridView();
            dgvReporte.Name = "dgvReporte";
            dgvReporte.Location = new Point(20, 200);
            dgvReporte.Size = new Size(1040, 200);
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Configurar columnas CON FECHA
            dgvReporte.ColumnCount = 6;
            dgvReporte.Columns[0].Name = "ID";
            dgvReporte.Columns[1].Name = "Cajero";
            dgvReporte.Columns[2].Name = "Fecha y Hora";       // Nueva columna
            dgvReporte.Columns[3].Name = "Ventas";
            dgvReporte.Columns[4].Name = "Propinas";
            dgvReporte.Columns[5].Name = "Total";

            // Ajustar anchos
            dgvReporte.Columns[0].Width = 40;   // ID
            dgvReporte.Columns[1].Width = 80;  // Cajero
            dgvReporte.Columns[2].Width = 130;  // Fecha y Hora
            dgvReporte.Columns[3].Width = 100;  // Ventas
            dgvReporte.Columns[4].Width = 100;  // Propinas
            dgvReporte.Columns[5].Width = 120;  // Total

            panelCorteCaja.Controls.Add(dgvReporte);

            Button btnExportarReporte = new Button();
            btnExportarReporte.Text = "📄 Exportar";
            btnExportarReporte.Location = new Point(100, 440);
            btnExportarReporte.Size = new Size(110, 45);
            btnExportarReporte.BackColor = Color.LightBlue;
            btnExportarReporte.Font = new Font("Arial", 9, FontStyle.Bold);
            btnExportarReporte.Click += (sender, e) =>
            {
                CorteCajaManager.ExportarReporte();
                
            };
            panelCorteCaja.Controls.Add(btnExportarReporte);


        }













        private void CargarDatosInventario(DataGridView dgv)
        {
            CargadorInventario.CargarDatosInventario(dgv);
        }

        private void FiltrarArticulosReabastecer(int stockMinimo)
        {
            DataGridView dgvArticulos = panelArticulosReabastecer.Controls.Find("dgvArticulosReabastecer", true).FirstOrDefault() as DataGridView;
            Label lblUrgentes = panelArticulosReabastecer.Controls.Find("lblUrgentes", true).FirstOrDefault() as Label;
            Label lblProximos = panelArticulosReabastecer.Controls.Find("lblProximos", true).FirstOrDefault() as Label;

            FiltroReabastecimiento.FiltrarArticulosReabastecer(dgvArticulos, lblUrgentes, lblProximos, stockMinimo);
        }

        private void ExportarListaReabastecimiento()
        {
            Inventario inventario = new Inventario();
            inventario.InicializarInventario();
            inventario.CargarInventario("inventario.json");
            NumericUpDown nudStockMinimo = panelArticulosReabastecer.Controls.Find("nudStockMinimo", true).FirstOrDefault() as NumericUpDown;
            int stockMinimo = nudStockMinimo?.Value != null ? (int)nudStockMinimo.Value : 5;

            ExportadorInventario.ExportarListaReabastecimiento(inventario, stockMinimo);
        }

        private void MostrarPanel(Panel panel)
        {
            // Ocultar todos los paneles
            OcultarTodosPaneles();

            // Mostrar solo el panel seleccionado
            panel.Visible = true;
            panel.BringToFront();

            // Actualizar datos automáticamente según el panel
            ActualizarDatosPanel(panel);
        }

        private void VerTodoElInventario_Click(object sender, EventArgs e)
        {

            MostrarPanel(panelVerInventario);
        }

        private void ArticulosAReabastecer_Click(object sender, EventArgs e)
        {
            MostrarPanel(panelArticulosReabastecer);
        }

        private void PedidoCedis_Click(object sender, EventArgs e)
        {
            MostrarPanel(panelPedidoCEDIS);
        }

        private void PedidoIprocurement_Click(object sender, EventArgs e)
        {
            MostrarPanel(panelPedidoIprocurement);
        }

        private void VerCajeros_Click(object sender, EventArgs e)
        {
            MostrarPanel(panelVerCajeros);
        }


        private void CorteDeCaja_Click(object sender, EventArgs e)
        {
            MostrarPanel(panelCorteCaja);
        }

        private void ActualizarDatosPanel(Panel panel)
        {
            try
            {
                // Actualizar panel de inventario
                if (panel == panelVerInventario)
                {
                    DataGridView dgvInventario = panel.Controls.Find("dgvInventario", true).FirstOrDefault() as DataGridView;
                    if (dgvInventario != null)
                    {
                        CargadorInventario.CargarDatosInventario(dgvInventario);
                    }
                }

                // Actualizar panel de artículos a reabastecer
                else if (panel == panelArticulosReabastecer)
                {
                    NumericUpDown nudStockMinimo = panel.Controls.Find("nudStockMinimo", true).FirstOrDefault() as NumericUpDown;
                    int stockMinimo = nudStockMinimo?.Value != null ? (int)nudStockMinimo.Value : 5;
                    FiltrarArticulosReabastecer(stockMinimo);
                }

                // Actualizar panel de pedido CEDIS
                else if (panel == panelPedidoCEDIS)
                {
                    DataGridView dgvPedidoCEDIS = panel.Controls.Find("dgvPedidoCEDIS", true).FirstOrDefault() as DataGridView;
                    if (dgvPedidoCEDIS != null)
                    {
                        PedidoCEDISManager.CargarProductosCEDIS(dgvPedidoCEDIS);
                    }
                }

                else if (panel == panelPedidoIprocurement)
                {
                    ActualizarTabsIprocurement();
                }

                // En el método ActualizarDatosPanel(), agrega:
                else if (panel == panelCorteCaja)
                {
                    // Actualizar estado de tickets
                    Label lblEstado = panel.Controls.Find("lblEstadoTickets", true).FirstOrDefault() as Label;
                    if (lblEstado != null)
                    {
                        lblEstado.Text = CorteCajaManager.ExistenTickets()
                            ? $"📄 {CorteCajaManager.ContarTickets()} tickets encontrados"
                            : "⚠️ No se encontraron tickets";
                        lblEstado.ForeColor = CorteCajaManager.ExistenTickets() ? Color.Green : Color.Red;
                    }

                    // Regenerar reporte si existen tickets
                    if (CorteCajaManager.ExistenTickets())
                    {
                        DataGridView dgv = panel.Controls.Find("dgvReporte", true).FirstOrDefault() as DataGridView;
                        Label lblVentas = panel.Controls.Find("lblTotalVentas", true).FirstOrDefault() as Label;
                        Label lblPropinas = panel.Controls.Find("lblTotalPropinas", true).FirstOrDefault() as Label;
                        Label lblGeneral = panel.Controls.Find("lblTotalGeneral", true).FirstOrDefault() as Label;
                        Label lblTickets = panel.Controls.Find("lblTotalTickets", true).FirstOrDefault() as Label;

                        if (dgv != null)
                        {
                            CorteCajaManager.GenerarReporte(dgv, lblVentas, lblPropinas, lblGeneral, lblTickets);
                        }
                    }
                }

                // Puedes agregar más paneles aquí según necesites
            }
            catch (Exception ex)
            {
                // Error silencioso para no interrumpir la navegación
                Console.WriteLine($"Error al actualizar datos del panel: {ex.Message}");
            }
        }
    }
}
