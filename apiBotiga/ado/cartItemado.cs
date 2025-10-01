using Microsoft.Data.SqlClient;
using botiga.Services;

namespace botiga.Repository;

class CartItemADO
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public void Add(DatabaseConnection dbConn)
    {
        dbConn.Open();
        string sql = @"INSERT INTO CartItems (Id, CartId, ProductId, Quantity)
                       VALUES (@Id, @CartId, @ProductId, @Quantity)";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@CartId", CartId);
        cmd.Parameters.AddWithValue("@ProductId", ProductId);
        cmd.Parameters.AddWithValue("@Quantity", Quantity);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static void Remove(DatabaseConnection dbConn, Guid cartId, Guid productId)
    {
        dbConn.Open();
        string sql = @"DELETE FROM CartItems WHERE CartId = @CartId AND ProductId = @ProductId";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@CartId", cartId);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
}
