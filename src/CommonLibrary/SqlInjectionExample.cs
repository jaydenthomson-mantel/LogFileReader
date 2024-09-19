using System.Data.SqlClient;

namespace CommonLibrary;

public static class SqlInjectionExample
{
    public static void GetUserData(string userId)
    {
        string connectionString = "your_connection_string_here";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Users WHERE UserId = '" + userId + "'";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["UserName"]);
            }
        }
    }
}