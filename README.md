# Изучение Entity Framework и ADO.NET 

#### Содержание:
- [Файл конфигурации App.config](#файл-конфигурации-appconfig) 
- [Entity Framework](#entity-framework)
  - [Создание модели User](#создание-модели-user)
  - [Контекст данных](#контекст-данных)
  - [Вывод данных в DataGrid в WPF](#вывод-данных-в-datagrid-в-wpf)
  - [Вывод данных в DataGridView в Windows Forms](#вывод-данных-в-datagridview-в-windows-forms)
- [ADO.NET](#adonet)
  - [Чтение данных из БД и их вывод](#чтение-данных-из-бд-и-их-вывод)
  - [Чтение данных из БД и их вывод в DataGrid в WPF](#чтение-данных-из-бд-и-их-вывод-в-datagrid-в-wpf)
  - [Чтение данных из БД и их вывод в DataGridView в Windows Forms](#чтение-данных-из-бд-и-их-вывод-в-datagridview-в-windows-forms)

## Файл конфигурации App.config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		<add
			 name="DefaultConnection"
			 connectionString="Server=localhost,63027;Database=testdb;Trusted_Connection=True"
			 providerName="System.Data.SqlClient"/>

		<add
			 name="ConnectionLocalDb"
			 connectionString="Server=(localdb)\mssqllocaldb;Database=testdb;Trusted_Connection=True;"
			 providerName="System.Data.SqlClient"/>

		<add 
			 name="ConnectionSQLite"
			 connectionString="Data Source=testdb.db"
			 providerName="System.Data.SQLite" />
	</connectionStrings>
</configuration>
```

## Entity Framework

### Создание модели User
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_console.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}

```

### Контекст данных 
```c#
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_console.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                // SQL Server connection with port
                //optionsBuilder.UseSqlServer("Server=localhost,63027;Database=UserDatabase;Trusted_Connection=True;");

                // SQL Server connection with localdb
                //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UserDatabase;Trusted_Connection=True;");

                // SQL Server connection from App.config
                //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnectionLocalDb"].ToString());

                // SQlite connection 
                //optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["ConnectionSQLite"].ToString());
                //optionsBuilder.UseSqlite(@"DataSource=ColledgeStore.db;");

            }
        }
    }
}
```

### Вывод данных в DataGrid в WPF 
```c#
 public partial class MainWindow : Window
    {
        ApplicationContext db;

        public MainWindow()
        {
            InitializeComponent();

            db = new ApplicationContext();
            db.Users.Load(); // загружаем данные
            usersGrid.ItemsSource = db.Users.Local.ToBindingList(); // устанавливаем привязку к кэшу

            this.Closing += MainWindow_Closing;
        }
    }
```

### Вывод данных в DataGridView в Windows Forms 
```c#
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
    }
```

## ADO.NET

### Чтение данных из БД и их вывод 
```c#
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;

namespace ado_console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionLocalDb"].ToString();
            string sqlExpression = "SELECT * FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    string columnName1 = reader.GetName(0);
                    string columnName2 = reader.GetName(1);
                    string columnName3 = reader.GetName(2);

                    Console.WriteLine($"{columnName1}\t{columnName2}\t{columnName3}");

                    while (await reader.ReadAsync()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        object age = reader.GetValue(2);

                        Console.WriteLine($"{id} \t{name} \t{age}");
                    }
                }

                await reader.CloseAsync();
            }
        }
    }
}
```

### Чтение данных из БД и их вывод в DataGrid в WPF 
```c#
private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                usersGrid.ItemsSource = usersTable.DefaultView;
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
```

### Чтение данных из БД и их вывод в DataGridView в Windows Forms 
```c#
public Form1()
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
```
