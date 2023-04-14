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

namespace Kvitancia
{
    public partial class FormDogovor : Form
    {
        DataTable lico = new DataTable();
        DataTable obrzovat = new DataTable();
        DataTable dogovor = new DataTable();
        DataTable pr = new DataTable();

        DataBase db = new DataBase();
        public FormDogovor()
        {
            InitializeComponent();
        }

        private void FormDogovor_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            lico.Clear();
            dogovor.Clear();
            obrzovat.Clear();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            db.OpenConnection();

            lico.Load(new SqlCommand("select * from FizLico", db.GetConnection()).ExecuteReader());
            dogovor.Load(new SqlCommand("select * from Dogovor", db.GetConnection()).ExecuteReader());
            obrzovat.Load(new SqlCommand("select * from ObrazovatelnayaProgramma", db.GetConnection()).ExecuteReader());


            string fio = "";
            string obraz = "";
            foreach(DataRow dr in lico.Rows)
            {
                fio = "";
                fio += dr.ItemArray[1] + " " + dr.ItemArray[2] + " " + dr.ItemArray[3];
                comboBox1.Items.Add(fio);
            }

            foreach(DataRow dr in obrzovat.Rows)
            {
                obraz = "";
                obraz += dr.ItemArray[1].ToString();
                comboBox2.Items.Add(obraz);
            }
            dataGridView1.DataSource = dogovor;
            db.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fio = comboBox1.SelectedItem.ToString();
            string obrazovanie = comboBox2.SelectedItem.ToString();
            string stoimost = textBox3.Text;
            string srok = textBox4.Text;
            string datanah = textBox5.Text;
            string datakonh = textBox6.Text;

            db.OpenConnection();
            new SqlCommand($"insert into dogovor values('{fio}', '{obrazovanie}', '{stoimost}', '{srok}', '{datanah}', '{datakonh}')", db.GetConnection()).ExecuteNonQuery();
            db.CloseConnection();

            UpdateData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            db.OpenConnection();
            pr.Load(new SqlCommand($"select * from ObrazovatelnayaProgramma where nazvanie = '{comboBox2.SelectedItem.ToString()}'", db.GetConnection()).ExecuteReader());

            textBox3.Text = pr.Rows[0].ItemArray[4].ToString();
            textBox4.Text = pr.Rows[0].ItemArray[2].ToString();

            db.CloseConnection();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox5.Text = e.Start.ToShortDateString();
            double days = double.Parse(textBox4.Text.Replace(" года", "")) * 365;

            textBox6.Text = e.Start.AddDays(days).ToShortDateString();
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox6.Text = e.Start.ToShortDateString();

            string date1 = textBox5.Text;
            string date2 = textBox6.Text;

            string resultDate = (DateTime.Parse(date2) - DateTime.Parse(date1)).Days.ToString();

            textBox4.Text = resultDate + " дней";
        }
    }
}
