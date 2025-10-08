using Microsoft.Data.SqlClient;

namespace botiga.Services;

public class DatabaseConnection
{
    
    public SqlConnection sqlConnection { get; private set; }

    private readonly string connectionString;

    public DatabaseConnection(string connectionString)
    {
        this.connectionString = connectionString;
        sqlConnection = new SqlConnection(connectionString);
    }

    public void Open()
    {
        if (sqlConnection.State != System.Data.ConnectionState.Open)
            sqlConnection.Open();
    }

    public void Close()
    {
        if (sqlConnection.State != System.Data.ConnectionState.Closed)
            sqlConnection.Close();
    }
}
