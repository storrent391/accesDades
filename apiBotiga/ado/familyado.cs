using Microsoft.Data.SqlClient;
using botiga.Services;

namespace botiga.Repository;

class FamilyADO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    public static void Insert(DatabaseConnection dbConn, FamilyADO family)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Families (Id, Name) VALUES (@Id, @Name)";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", family.Id);
        cmd.Parameters.AddWithValue("@Name", family.Name);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static List<FamilyADO> GetAll(DatabaseConnection dbConn)
    {
        List<FamilyADO> list = new();
        dbConn.Open();

        string sql = "SELECT Id, Name FROM Families";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new FamilyADO
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
            });
        }

        dbConn.Close();
        return list;
    }

    public static FamilyADO? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = "SELECT * FROM Families WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        FamilyADO? family = null;

        if (reader.Read())
        {
            family = new FamilyADO
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
            };
        }

        dbConn.Close();
        return family;
    }

    public static void Update(DatabaseConnection dbConn, FamilyADO family)
    {
        dbConn.Open();

        string sql = @"UPDATE Families
                    SET Name = @Name,
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", family.Id);
        cmd.Parameters.AddWithValue("@Name", family.Name);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} fila actualitzada.");

        dbConn.Close();
    }
    

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Families WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}
