namespace Examen_Unidad3
{
    partial class ComedorLlevar
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
            button1 = new Button();
            textBox1 = new TextBox();
            panel1 = new Panel();
            textBox2 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Black", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(840, 45);
            button1.Name = "button1";
            button1.Size = new Size(1242, 547);
            button1.TabIndex = 0;
            button1.Text = "Comedor";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 855);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(772, 494);
            textBox1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(778, 1445);
            panel1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 3);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(772, 813);
            textBox2.TabIndex = 2;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI Black", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(840, 612);
            button2.Name = "button2";
            button2.Size = new Size(1242, 547);
            button2.TabIndex = 3;
            button2.Text = "Llevar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI Black", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(2088, 1197);
            button3.Name = "button3";
            button3.Size = new Size(428, 180);
            button3.TabIndex = 4;
            button3.Text = "Cerrar sesion";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // ComedorLlevar
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(2534, 1469);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(panel1);
            Controls.Add(button1);
            ForeColor = Color.White;
            Name = "ComedorLlevar";
            Text = "ComedorLlevar";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Panel panel1;
        private TextBox textBox2;
        private Button button2;
        private Button button3;
    }
}