namespace Examen_Unidad3.Administrador
{
    partial class MenuAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            inventarioToolStripMenuItem = new ToolStripMenuItem();
            VerTodoElInventario = new ToolStripMenuItem();
            ArticulosAReabastecer = new ToolStripMenuItem();
            pOSAdminToolStripMenuItem = new ToolStripMenuItem();
            PedidoCedis = new ToolStripMenuItem();
            PedidoIprocurement = new ToolStripMenuItem();
            pOSAdminToolStripMenuItem1 = new ToolStripMenuItem();
            VerCajeros = new ToolStripMenuItem();
            CorteDeCaja = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { inventarioToolStripMenuItem, pOSAdminToolStripMenuItem, pOSAdminToolStripMenuItem1, CorteDeCaja });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1099, 40);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // inventarioToolStripMenuItem
            // 
            inventarioToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { VerTodoElInventario, ArticulosAReabastecer });
            inventarioToolStripMenuItem.Name = "inventarioToolStripMenuItem";
            inventarioToolStripMenuItem.Size = new Size(141, 36);
            inventarioToolStripMenuItem.Text = "Inventario";
            // 
            // VerTodoElInventario
            // 
            VerTodoElInventario.Name = "VerTodoElInventario";
            VerTodoElInventario.Size = new Size(387, 44);
            VerTodoElInventario.Text = "Ver todo el inventario";
            VerTodoElInventario.Click += VerTodoElInventario_Click;
            // 
            // ArticulosAReabastecer
            // 
            ArticulosAReabastecer.Name = "ArticulosAReabastecer";
            ArticulosAReabastecer.Size = new Size(387, 44);
            ArticulosAReabastecer.Text = "Articulos a reabastecer";
            ArticulosAReabastecer.Click += ArticulosAReabastecer_Click;
            // 
            // pOSAdminToolStripMenuItem
            // 
            pOSAdminToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { PedidoCedis, PedidoIprocurement });
            pOSAdminToolStripMenuItem.Name = "pOSAdminToolStripMenuItem";
            pOSAdminToolStripMenuItem.Size = new Size(117, 36);
            pOSAdminToolStripMenuItem.Text = "Pedidos";
            // 
            // PedidoCedis
            // 
            PedidoCedis.Name = "PedidoCedis";
            PedidoCedis.Size = new Size(371, 44);
            PedidoCedis.Text = "Pedido CEDIS";
            PedidoCedis.Click += PedidoCedis_Click;
            // 
            // PedidoIprocurement
            // 
            PedidoIprocurement.Name = "PedidoIprocurement";
            PedidoIprocurement.Size = new Size(371, 44);
            PedidoIprocurement.Text = "Pedido Iprocurement";
            PedidoIprocurement.Click += PedidoIprocurement_Click;
            // 
            // pOSAdminToolStripMenuItem1
            // 
            pOSAdminToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { VerCajeros });
            pOSAdminToolStripMenuItem1.Name = "pOSAdminToolStripMenuItem1";
            pOSAdminToolStripMenuItem1.Size = new Size(155, 36);
            pOSAdminToolStripMenuItem1.Text = "POS Admin";
            // 
            // VerCajeros
            // 
            VerCajeros.Name = "VerCajeros";
            VerCajeros.Size = new Size(359, 44);
            VerCajeros.Text = "Ver cajeros";
            VerCajeros.Click += VerCajeros_Click;
            // 
            // CorteDeCaja
            // 
            CorteDeCaja.Name = "CorteDeCaja";
            CorteDeCaja.Size = new Size(174, 36);
            CorteDeCaja.Text = "Corte de caja";
            CorteDeCaja.Click += CorteDeCaja_Click;
            // 
            // MenuAdmin
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1099, 712);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MenuAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MenuAdmin";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem inventarioToolStripMenuItem;
        private ToolStripMenuItem pOSAdminToolStripMenuItem;
        private ToolStripMenuItem pOSAdminToolStripMenuItem1;
        private ToolStripMenuItem VerTodoElInventario;
        private ToolStripMenuItem ArticulosAReabastecer;
        private ToolStripMenuItem PedidoCedis;
        private ToolStripMenuItem PedidoIprocurement;
        private ToolStripMenuItem VerCajeros;
        private ToolStripMenuItem CorteDeCaja;
    }
}