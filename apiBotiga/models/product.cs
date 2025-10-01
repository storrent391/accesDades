namespace botiga.Model;

public class Product
{
    public Guid Id { get; set; }
    public Guid FamilyId { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
}
