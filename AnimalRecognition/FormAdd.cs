using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimalRecognition
{
    class FormAdd : Form
    {
        private Form1 MainForm { get; set; }
        public FormAdd(Form1 previousForm)
        {
            MainForm = previousForm;
            InitializeComponent();
        }

        private TextBox textBox1;
        private Button button1;
        private Button button2;
        string TextBoxname;
        private void OkButton_Click(object sender, EventArgs e)
        {

            TextBoxname = textBox1.Text;
            if (textBox1.Text == "")
            {
                TextBoxname = "Animal" + MainForm.NumberId();
            }
            MainForm.AddToGrid(TextBoxname);
            MainForm.gridFill();
            ActiveForm.Close();
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(260, 22);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(260, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormAdd
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "FormAdd";
            this.Text = "Add To DataBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //Close Form
        private void button2_Click(object sender, EventArgs e)
        { ActiveForm.Close(); }
    }
}
