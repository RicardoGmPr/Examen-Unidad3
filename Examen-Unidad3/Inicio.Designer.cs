namespace Examen_Unidad3
{
    partial class Inicio
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            button1 = new Button();
            label2 = new Label();
            button2 = new Button();
            lblFechaHora = new Label();
            timerFechaHora = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Logo_Blanco;
            pictureBox1.Location = new Point(17, 21);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(302, 101);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 19.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Yellow;
            label1.Location = new Point(-13, 74);
            label1.Name = "label1";
            label1.Size = new Size(1284, 71);
            label1.TabIndex = 1;
            label1.Text = "_________________________________________________________";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Yellow;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Variable Display", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Image = Properties.Resources.Logo_Comida1;
            button1.Location = new Point(310, 247);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(624, 167);
            button1.TabIndex = 2;
            button1.Text = "   Punto de venta";
            button1.TextAlign = ContentAlignment.MiddleLeft;
            button1.TextImageRelation = TextImageRelation.TextBeforeImage;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 6F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(21, 738);
            label2.Name = "label2";
            label2.Size = new Size(1203, 21);
            label2.TabIndex = 3;
            label2.Text = "© 2025 CARL KARCHER ENTERPRISES, INC. ALGUNOS PRODUCTOS ESTAN SUJETOS A DISPONIBILIDAD DEPENDIENDO DEL RESTAURANTE Y/O CIUDAD.";
            label2.Click += label2_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Yellow;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI Variable Display", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Image = Properties.Resources.Icon_Admin;
            button2.Location = new Point(310, 457);
            button2.Name = "button2";
            button2.Size = new Size(624, 147);
            button2.TabIndex = 4;
            button2.Text = "   Administrador";
            button2.TextAlign = ContentAlignment.MiddleLeft;
            button2.TextImageRelation = TextImageRelation.TextBeforeImage;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // lblFechaHora
            // 
            lblFechaHora.AutoSize = true;
            lblFechaHora.Font = new Font("Segoe UI Black", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFechaHora.ForeColor = Color.White;
            lblFechaHora.Location = new Point(725, 90);
            lblFechaHora.Name = "lblFechaHora";
            lblFechaHora.Size = new Size(470, 41);
            lblFechaHora.TabIndex = 5;
            lblFechaHora.Text = "Lunes, 21 Abril 2025 - 14:00:45";
            lblFechaHora.TextAlign = ContentAlignment.MiddleCenter;
            lblFechaHora.Click += lblFechaHora_Click;
            // 
            // timerFechaHora
            // 
            timerFechaHora.Enabled = true;
            timerFechaHora.Interval = 1000;
            timerFechaHora.Tick += timerFechaHora_Tick;
            // 
            // Inicio
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1244, 769);
            Controls.Add(lblFechaHora);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Inicio";
            Text = "Carls Jr";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Button button1;
        private Label label2;
        private Button button2;
        private Label lblFechaHora;
        private System.Windows.Forms.Timer timerFechaHora;
    }
}
