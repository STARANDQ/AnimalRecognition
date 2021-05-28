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
    public partial class Form1 : Form
    {
        public Network ReadyToUseNetwork = new Network();
        
        FormAdd DialogAdd;
        FormDelete DialogDelete;

        bool[] allCheckBox = new bool[Controller.countCheckBox];
        CheckBox[] arrayCheckBox = new CheckBox[Controller.countCheckBox];

        double wage;
        public void gridFill()
        {
            this.animalRenTableAdapter.Fill(this.animalDataSet.AnimalRen);
        }
        public Form1()
        {
            InitializeComponent();
            addCheckBoxInArray();
            gridFill();
        }
        
        public void ShowMyDialogBox()
        {
            DialogAdd = new FormAdd(this);
            DialogResult dialog = DialogAdd.ShowDialog(this);
        }

        
        public void ShowMyDeleteForm()
        {
            DialogDelete = new FormDelete(this);
            DialogResult dialog = DialogDelete.ShowDialog(this);
        }

        public int NumberId()
        {
            SqlConnection SQL = new SqlConnection(Controller.connectSql);
            SQL.Open();
            string sendCommand = "SELECT top 1 * FROM " + Controller.table + " order by Id Desc;";
            
            SqlCommand command = new SqlCommand(sendCommand, SQL);
            
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id = Convert.ToInt32(String.Format("{0}", reader["id"])) + 1;
                    SQL.Close();
                    return id;
                }
                else return 0;
            }

        }
        public void AddToGrid(string name)
        {
            int[] allCheckBoxTemp = new int[Controller.countCheckBox];
            for (int i = 0; i < Controller.countCheckBox; i++)
            {
                if (allCheckBox[i]) allCheckBoxTemp[i] = 1;
                else allCheckBoxTemp[i] = 0;
            }
            
            SqlConnection SQL = new SqlConnection(Controller.connectSql);
            SQL.Open();
            
            string sendCommand = commandInsert(allCheckBoxTemp, name);
            
            SqlCommand command = new SqlCommand(sendCommand, SQL);
            
            command.ExecuteNonQuery();
            SQL.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Color red = new Color();
            red = Color.FromArgb(255, 0, 0);

            Color black = new Color();
            black = Color.FromArgb(0, 0, 0);

            allCheckBox = infoCheckBox();

            int checkItem = 0;
            for (int i = 0; i < Controller.countCheckBox; i++)
                if (!allCheckBox[i]) checkItem++;

            if (checkItem != Controller.countCheckBox)
            {
                ShowMyDialogBox();
                for (int i = 0; i < Controller.countCheckBox; i++)
                    arrayCheckBox[i].ForeColor = black;
            }
            else
            {
                for (int i = 0; i < Controller.countCheckBox; i++)
                    arrayCheckBox[i].ForeColor = red;
            }

            clearCheckBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vector[] X = new Vector[dataGridView1.RowCount];

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Vector k = new Vector(Controller.countCheckBox);
                for (int j = 1; j < 33; j++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[j + 1].Value) == true)
                    {
                        if (j + 1 == 1 || j + 1 == 2 || j + 1 == 3 || j + 1 == 4 || j + 1 == 5 || j + 1 == 6 || j + 1 == 7 || j + 1 == 8 || j + 1 == 10 || j + 1 == 22)
                            wage = 0.6;
                        if (j + 1 == 12 || j + 1 == 13 || j + 1 == 14 || j + 1 == 20 || j + 1 == 23 || j + 1 == 28 || j + 1 == 31 || j + 1 == 18)
                            wage = 0.8;
                        if (j + 1 == 9 || j + 1 == 11 || j + 1 == 25 || j + 1 == 24 || j + 1 == 32 || j + 1 == 26)
                            wage = 0.3;
                        if (j + 1 == 15 || j + 1 == 16 | j + 1 == 17 || j + 1 == 21 || j + 1 == 19 || j + 1 == 27 || j + 1 == 30 || j + 1 == 29)
                            wage = 0.5;
                        k.vector[j - 1] = 1 * wage;
                    }
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[j + 1].Value) == false)
                    {
                        k.vector[j - 1] = 0;
                    }
                }
                X[i] = k;
            }
            Vector[] Y = new Vector[dataGridView1.RowCount];
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Vector k = new Vector(dataGridView1.RowCount);
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    if (i == j) { k.vector[j] = 1.0; } else { k.vector[j] = 0.0; }
                }
                Y[i] = k;
            }
            Network network = new Network(new int[] { Controller.countCheckBox, 16, dataGridView1.RowCount });
            network.Train(X, Y, 0.1, 1e-7, 10000);
            ReadyToUseNetwork = network;
            button3.Enabled = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool[] checkBoxes = new bool[Controller.countCheckBox];

            checkBoxes = infoCheckBox();

            Vector vtry = new Vector(Controller.countCheckBox);
            
            for (int i = 0; i < Controller.countCheckBox; i++)
            {
                if (checkBoxes[i]) 
                { vtry.vector[i] = 1; } else { vtry.vector[i] = 0; }
            }

            Vector vector = null;
            vector = ReadyToUseNetwork.Forward(vtry);

            double max = 0;
            int indexMax = 0;
            for (int j = 0; j < dataGridView1.RowCount - 1; j++)
            {
                if (vector[j] > max)
                {
                    max = vector[j];
                    indexMax = j;
                }
            }
            label2.Text = $"It's close to {Math.Round(max * 100, 2)}% " + dataGridView1.Rows[indexMax].Cells[1].Value;

        }

        private void button4_Click(object sender, EventArgs e) { ShowMyDeleteForm(); }

        private bool[] infoCheckBox()
        {
            bool[] array = new bool[Controller.countCheckBox];

            for(int i = 0; i < Controller.countCheckBox; i++)
            {
                array[i] = arrayCheckBox[i].Checked;
            }

            return array;
        }

        private void clearCheckBox()
        {
            for(int i = 0; i < Controller.countCheckBox; i++)
            {
                arrayCheckBox[i].Checked = false;
            }
        }

        private string commandInsert(int[] array, string name)
        {
            string result = "";
            string resultProperty = "";
            string[] arrayAnimalProperty = new string[Controller.countCheckBox];

            arrayAnimalProperty = getarrayAnimalProperty();

            for (int i = 0; i < Controller.countCheckBox - 1; i++)
            {
                result += array[i] + ", ";
                resultProperty += $"{arrayAnimalProperty[i]}, ";
            }
            result += array[Controller.countCheckBox - 1];
            resultProperty += arrayAnimalProperty[Controller.countCheckBox - 1];


            return $"INSERT INTO {Controller.table} " +

                $"(Id, " +
                $"Name, " +
                $"{resultProperty}) " +

                $"VALUES(" +

                $"{NumberId()}, " +
                $"'{name}', " +
                $"{result});"; 

        }

        private string[] getarrayAnimalProperty()
        {
            string[] property = new string[Controller.countCheckBox];

            property[0] = "Wool";
            property[1] = "Skin";
            property[2] = "Plumage";
            property[3] = "Shell";
            property[4] = "Wings";
            property[5] = "Scale";
            property[6] = "Fins";
            property[7] = "Paws";
            property[8] = "Hooves";
            property[9] = "Horns";
            property[10] = "Tail";
            property[11] = "Trunk";
            property[12] = "Omnivorous";
            property[13] = "Herbivorous";
            property[14] = "Carnivorous";
            property[15] = "Poisonous";
            property[16] = "Sanguivorous";
            property[17] = "Mammal";
            property[18] = "Bird";
            property[19] = "Fish";
            property[20] = "Amphibious";
            property[21] = "Insect";
            property[22] = "Arachnid";
            property[23] = "Worm";
            property[24] = "Shellfish";
            property[25] = "Rodent";
            property[26] = "Cetaceans";
            property[27] = "Primate";
            property[28] = "Huge";
            property[29] = "Big";
            property[30] = "Middle";
            property[31] = "Small";
            property[32] = "Tiny";
            property[33] = "Pet";
            property[34] = "Savage";

            return property;
        }

        private void addCheckBoxInArray()
        {
            arrayCheckBox[0] = checkBox1;
            arrayCheckBox[1] = checkBox2;
            arrayCheckBox[2] = checkBox3;
            arrayCheckBox[3] = checkBox4;
            arrayCheckBox[4] = checkBox5;
            arrayCheckBox[5] = checkBox6;
            arrayCheckBox[6] = checkBox7;
            arrayCheckBox[7] = checkBox8;
            arrayCheckBox[8] = checkBox9;
            arrayCheckBox[9] = checkBox10;
            arrayCheckBox[10] = checkBox11;
            arrayCheckBox[11] = checkBox12;
            arrayCheckBox[12] = checkBox13;
            arrayCheckBox[13] = checkBox14;
            arrayCheckBox[14] = checkBox15;
            arrayCheckBox[15] = checkBox16;
            arrayCheckBox[16] = checkBox17;
            arrayCheckBox[17] = checkBox18;
            arrayCheckBox[18] = checkBox19;
            arrayCheckBox[19] = checkBox20;
            arrayCheckBox[20] = checkBox21;
            arrayCheckBox[21] = checkBox22;
            arrayCheckBox[22] = checkBox23;
            arrayCheckBox[23] = checkBox24;
            arrayCheckBox[24] = checkBox25;
            arrayCheckBox[25] = checkBox26;
            arrayCheckBox[26] = checkBox27;
            arrayCheckBox[27] = checkBox28;
            arrayCheckBox[28] = checkBox29;
            arrayCheckBox[29] = checkBox30;
            arrayCheckBox[30] = checkBox31;
            arrayCheckBox[31] = checkBox32;
            arrayCheckBox[32] = checkBox33;
            arrayCheckBox[33] = checkBox34;
            arrayCheckBox[34] = checkBox35;

            string[] arrayAnimalProperty = new string[Controller.countCheckBox];

            arrayAnimalProperty = getarrayAnimalProperty();

            for (int i = 0; i < Controller.countCheckBox; i++)
                arrayCheckBox[i].Text = arrayAnimalProperty[i];

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "animalDataSet.AnimalRen". При необходимости она может быть перемещена или удалена.
            this.animalRenTableAdapter.Fill(this.animalDataSet.AnimalRen);

        }
    }
}
