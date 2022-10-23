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