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
    public partial class FormAuth : Form
    {
        DataBase DataBase = new DataBase();
        private int count;
        public FormAuth()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var pass = textBox2.Text;

            DataTable table = new DataTable();

            string loginquery = $"select login, password, isAdmin from users where login = '{login}' and password = '{pass}'";

            DataBase.OpenConnection();

            SqlCommand command = new SqlCommand(loginquery, DataBase.GetConnection());

            table.Load(command.ExecuteReader());

            DataBase.CloseConnection();
            if (table.Rows.Count > 0)
            {
                new Form1().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");

                if (count++ == 2)
                {
                    FormSbros fs = new FormSbros();
                    this.Hide();
                    fs.Show();
                }
            }
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {

        }
    }
}
