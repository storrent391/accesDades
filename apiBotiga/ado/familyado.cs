using Microsoft.Data.SqlClient;
using botiga.Services;

namespace botiga.Repository;

class FamilyADO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";

    public void Insert(DatabaseConnection dbConn)
    {
        dbConn.Open();
        string sql = @"INSERT INTO Families (Id, Name) VALUES (@Id, @Name)";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@Name", Name);
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
}
