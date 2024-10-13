namespace EpohUI.Lib
{
    public class SQLiteHelper
    {

        public static string select(string reqBody)
        {
            string connectionString = "Data Source=yourdatabase.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO your_table (column1, column2) VALUES (@value1, @value2)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@value1", "data1");
                    command.Parameters.AddWithValue("@value2", "data2");
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
