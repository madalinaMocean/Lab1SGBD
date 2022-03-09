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
            this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);

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
            if (e.RowIndex != -1)
            {
                string HotelId;
                object value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                if (value is DBNull) { return; }

                HotelId = value.ToString();
                RetrieveCopiesHotelId(HotelId);
            }

        }

        private void RetrieveCopiesHotelId(string HotelId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlDataAdapter da = new SqlDataAdapter("select * from Employee where HotelId = @HotelId", connection);
                da.SelectCommand.Parameters.AddWithValue("@HotelId", HotelId);

                DataTable dtbl2 = new DataTable();
                da.Fill(dtbl2);
                dataGridView2.DataSource = dtbl2;
                connection.Close();
            }
        }


        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    int iRownr = this.dataGridView1.CurrentCell.RowIndex;

        //    object cellvalue1 = this.dataGridView1[0, iRownr].Value;
        //    string IdString = cellvalue1.ToString();
        //    int Id = Int32.Parse(IdString);
        //    HotelIdTextBox.Text = IdString;
        //    insertButton.Enabled = true;
        //    updateButton.Enabled = false;
        //    deleteButton.Enabled = false;
        //    HotelIdTextBox.ReadOnly = true;

        //    //create connection
        //    SqlConnection connection = new SqlConnection(connectionString);

        //    //create the command and parameter objects
        //    string childQuery = "select * from Employee where HotelId = @HotelId";
        //    SqlCommand command = new SqlCommand(childQuery, connection);
        //    command.Parameters.Add("@HotelId",SqlDbType.Int);
        //    command.Parameters["@HotelId"].Value=Id;

        //    try
        //    {
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        DataTable childTable = new DataTable();
        //        childTable.Load(reader);
        //        dataGridView2.DataSource = childTable;
        //        reader.Close();
        //    }

        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        connection.Close();
        //    }


        //}
    }
}
