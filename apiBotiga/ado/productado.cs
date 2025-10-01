using Microsoft.Data.SqlClient;
using botiga.Services;

namespace botiga.Repository;

class ProductADO
{
    public Guid Id { get; set; }
    public Guid FamilyId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

    public void Insert(DatabaseConnection dbConn)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Products (Id, FamilyId, Code, Name, Price, Discount)
                       VALUES (@Id, @FamilyId, @Code, @Name, @Price, @Discount)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@FamilyId", FamilyId);
        cmd.Parameters.AddWithValue("@Code", Code);
        cmd.Parameters.AddWithValue("@Name", Name);
        cmd.Parameters.AddWithValue("@Price", Price);
        cmd.Parameters.AddWithValue("@Discount", Discount);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static List<ProductADO> GetAll(DatabaseConnection dbConn)
    {
        List<ProductADO> list = new();
        dbConn.Open();

        string sql = "SELECT Id, FamilyId, Code, Name, Price, Discount FROM Products";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new ProductADO
            {
                Id = reader.GetGuid(0),
                FamilyId = reader.GetGuid(1),
                Code = reader.GetString(2),
                Name = reader.GetString(3),
                Price = reader.GetDecimal(4),
                Discount = reader.GetDecimal(5)
            });
        }

        dbConn.Close();
        return list;
    }

    public static ProductADO? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, FamilyId, Code, Name, Price, Discount FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        ProductADO? product = null;

        if (reader.Read())
        {
            product = new ProductADO
            {
                Id = reader.GetGuid(0),
                FamilyId = reader.GetGuid(1),
                Code = reader.GetString(2),
                Name = reader.GetString(3),
                Price = reader.GetDecimal(4),
                Discount = reader.GetDecimal(5)
            };
        }

        dbConn.Close();
        return product;
    }
}
