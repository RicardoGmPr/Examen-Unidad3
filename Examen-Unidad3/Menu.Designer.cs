namespace Examen_Unidad3
{
    partial class Menu
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
            panel1 = new Panel();
            listBoxTicket = new ListBox();
            textBox1 = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button5 = new Button();
            button6 = new Button();
            button4 = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            btnEliminarSeleccionado = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(listBoxTicket);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(778, 1445);
            panel1.TabIndex = 3;
            // 
            // listBoxTicket
            // 
            listBoxTicket.Font = new Font("Consolas", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBoxTicket.FormattingEnabled = true;
            listBoxTicket.ItemHeight = 43;
            listBoxTicket.Location = new Point(3, 3);
            listBoxTicket.Name = "listBoxTicket";
            listBoxTicket.Size = new Size(772, 778);
            listBoxTicket.TabIndex = 0;
            listBoxTicket.SelectedIndexChanged += listBoxTicket_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 922);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(772, 494);
            textBox1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(button5);
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Controls.Add(button4);
            flowLayoutPanel1.Location = new Point(803, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1716, 131);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(280, 112);
            button1.TabIndex = 0;
            button1.Text = "Hamburguesas";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(289, 3);
            button2.Name = "button2";
            button2.Size = new Size(280, 112);
            button2.TabIndex = 1;
            button2.Text = "Bebidas";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(575, 3);
            button3.Name = "button3";
            button3.Size = new Size(280, 112);
            button3.TabIndex = 2;
            button3.Text = "Fritos";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button1_Click;
            // 
            // button5
            // 
            button5.Location = new Point(861, 3);
            button5.Name = "button5";
            button5.Size = new Size(280, 112);
            button5.TabIndex = 4;
            button5.Text = "Postres";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button1_Click;
            // 
            // button6
            // 
            button6.Location = new Point(1147, 3);
            button6.Name = "button6";
            button6.Size = new Size(280, 112);
            button6.TabIndex = 5;
            button6.Text = "Extras";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button1_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1433, 3);
            button4.Name = "button4";
            button4.Size = new Size(280, 112);
            button4.TabIndex = 3;
            button4.Text = "Combo Hmbrg";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button1_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = SystemColors.ControlLight;
            flowLayoutPanel2.Location = new Point(803, 162);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(1716, 943);
            flowLayoutPanel2.TabIndex = 5;
            // 
            // btnEliminarSeleccionado
            // 
            btnEliminarSeleccionado.Location = new Point(806, 1141);
            btnEliminarSeleccionado.Name = "btnEliminarSeleccionado";
            btnEliminarSeleccionado.Size = new Size(231, 107);
            btnEliminarSeleccionado.TabIndex = 6;
            btnEliminarSeleccionado.Text = "Eliminar";
            btnEliminarSeleccionado.UseVisualStyleBackColor = true;
            btnEliminarSeleccionado.Click += btnEliminarSeleccionado_Click;
            // 
            // button7
            // 
            button7.Location = new Point(1061, 1141);
            button7.Name = "button7";
            button7.Size = new Size(236, 107);
            button7.TabIndex = 7;
            button7.Text = "Agregar";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Enabled = false;
            button8.Location = new Point(1315, 1141);
            button8.Name = "button8";
            button8.Size = new Size(236, 107);
            button8.TabIndex = 8;
            button8.Text = "Quitar";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(2263, 1182);
            button9.Name = "button9";
            button9.Size = new Size(236, 107);
            button9.TabIndex = 9;
            button9.Text = "Finalizar";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(2263, 1321);
            button10.Name = "button10";
            button10.Size = new Size(236, 107);
            button10.TabIndex = 10;
            button10.Text = "Salir";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2534, 1469);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(btnEliminarSeleccionado);
            Controls.Add(flowLayoutPanel2);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Name = "Menu";
            Text = "Menu";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox textBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private FlowLayoutPanel flowLayoutPanel2;
        private ListBox listBoxTicket;
        private Button btnEliminarSeleccionado;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
    }
}