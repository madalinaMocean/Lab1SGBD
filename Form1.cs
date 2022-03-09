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

namespace SGBDLab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            insertButton.Enabled = false;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
           
        }

        string connectionString = "Data Source=DESKTOP-UV4O3H6\\SQLEXPRESS;Initial Catalog=HotelManagement;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter;

            string hotelQuery = "select * from Hotel";
            string employeeQuery = "select * from Employee";
            //create a DataSet
            DataSet ds1 = new DataSet();

            try
            {
                //open connection and create DataAdapter
                connection.Open();
                adapter = new SqlDataAdapter(hotelQuery, connection);
                adapter.Fill(ds1,"Hotel");
                adapter = new SqlDataAdapter(employeeQuery, connection);
                adapter.Fill(ds1,"Employee");

                ds1.Relations.Add("Hotel Employee",
                ds1.Tables["Hotel"].Columns["HotelId"],
                ds1.Tables["Employee"].Columns["HotelId"]);
                connection.Close();

                dataGridView1.DataSource = ds1.Tables["Hotel"];

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRownr = this.dataGridView1.CurrentCell.RowIndex;

            object cellvalue1 = this.dataGridView1[0, iRownr].Value;
            string IdString = cellvalue1.ToString();
            int Id = Int32.Parse(IdString);
            HotelIdTextBox.Text = IdString;
            insertButton.Enabled = true;
            updateButton.Enabled = false;
            deleteButton.Enabled = false;
            HotelIdTextBox.ReadOnly = true;

            //create connection
            SqlConnection connection = new SqlConnection(connectionString);

            //create the command and parameter objects
            string childQuery = "select * from Employee where HotelId = @HotelId";
            SqlCommand command = new SqlCommand(childQuery, connection);
            command.Parameters.Add("@HotelId",SqlDbType.Int);
            command.Parameters["@HotelId"].Value=Id;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                DataTable childTable = new DataTable();
                childTable.Load(reader);
                dataGridView2.DataSource = childTable;
                reader.Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }


        }
    }
}
