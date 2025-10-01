using Microsoft.Data.SqlClient;

namespace botiga.Services;

public class DatabaseConnection
{
    private readonly string _connectionString;
    public SqlConnection sqlConnection;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool Open()
    {
        sqlConnection = new SqlConnection(_connectionString);
        try
        {
            sqlConnection.Open();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to database: {ex.Message}");
            return false;
        }
    }

    public void Close()
    {
        if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            sqlConnection.Close();
    }
}
