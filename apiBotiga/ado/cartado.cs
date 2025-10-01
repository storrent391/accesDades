using Microsoft.Data.SqlClient;
using botiga.Services;

namespace botiga.Repository;

class CartADO
{
    public Guid Id { get; set; }

    public void Create(DatabaseConnection dbConn)
    {
        dbConn.Open();
        string sql = @"INSERT INTO Carts (Id) VALUES (@Id)";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
}
