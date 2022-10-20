using ef_winforms.Models;
using System.Data.Entity;
using ApplicationContext = ef_winforms.Models.ApplicationContext;

namespace ef_winforms
{
    public partial class Form1 : Form
    {
        ApplicationContext db = new ApplicationContext();

        public Form1()
        {
            InitializeComponent();
            db.Users.Load();
            dataGridView1.DataSource = db.Users.Local.ToBindingList();
            dataGridView1.Columns[0].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.SaveChanges();
        }
    }
}