namespace Examen_Unidad3
{
    partial class Venta
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
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            button1 = new Button();
            label3 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            textBox4 = new TextBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(290, 37);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(325, 61);
            textBox1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(136, 39);
            label1.Name = "label1";
            label1.Size = new Size(148, 59);
            label1.TabIndex = 1;
            label1.Text = "Total:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(136, 133);
            label2.Name = "label2";
            label2.Size = new Size(146, 59);
            label2.TabIndex = 2;
            label2.Text = "Pago:";
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(290, 131);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(325, 61);
            textBox2.TabIndex = 3;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Black", 13.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(55, 307);
            button1.Name = "button1";
            button1.Size = new Size(188, 73);
            button1.TabIndex = 4;
            button1.Text = "Finalizar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(322, 314);
            label3.Name = "label3";
            label3.Size = new Size(199, 59);
            label3.TabIndex = 5;
            label3.Text = "Cambio:";
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox3.Location = new Point(542, 312);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(215, 61);
            textBox3.TabIndex = 6;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(79, 219);
            label4.Name = "label4";
            label4.Size = new Size(205, 59);
            label4.TabIndex = 7;
            label4.Text = "Propina:";
            // 
            // textBox4
            // 
            textBox4.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox4.Location = new Point(290, 217);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(215, 61);
            textBox4.TabIndex = 8;
            // 
            // Venta
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 479);
            Controls.Add(textBox4);
            Controls.Add(label4);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Name = "Venta";
            Text = "Venta";
            Load += Venta_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Button button1;
        private Label label3;
        public TextBox textBox1;
        public TextBox textBox2;
        public TextBox textBox3;
        private Label label4;
        public TextBox textBox4;
    }
}