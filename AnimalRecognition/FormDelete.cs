using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimalRecognition
{
    class FormDelete : Form
    {
        private Button button1;
        private Button button2;
        private TextBox textBox1;

        private Form1 MainForm { get; set; }
        public FormDelete(Form1 previousForm)
        {
            MainForm = previousForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection SQL = new SqlConnection("Data Source = " + Controller.dataSource + "; Initial Catalog = " + Controller.catalog + "; Integrated Security = " + Controller.security);
            SQL.Open();
            string send = $"DELETE FROM {Controller.table} WHERE Id = " + textBox1.Text;
            SqlCommand command = new SqlCommand(send, SQL);
            command.ExecuteNonQuery();
            SQL.Close();
            MainForm.gridFill();
            ActiveForm.Close();
        }

        private void button2_Click(object sender, EventArgs e) { ActiveForm.Close(); }

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
            this.textBox1.Size = new System.Drawing.Size(260, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(260, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancle";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormDelete
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "FormDelete";
            this.Text = "Delete From DataBase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
