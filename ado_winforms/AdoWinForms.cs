using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ado_winforms
{
    public partial class AdoWinForms : Form
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable usersTable;

        public AdoWinForms()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ConnectionLocalDb"].ToString();

            string sqlExpression = "SELECT * FROM Users";
            usersTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(usersTable);
                dataGridView1.DataSource = usersTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}