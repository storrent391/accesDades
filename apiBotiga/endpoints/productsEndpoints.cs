using botiga.Repository;
using botiga.Services;

namespace botiga.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /products
        app.MapGet("/products", () =>
        {
            var products = ProductADO.GetAll(dbConn);
            return Results.Ok(products);
        });

        // GET /products/{id}
        app.MapGet("/products/{id}", (Guid id) =>
        {
            var product = ProductADO.GetById(dbConn, id);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound(new { message = $"Product with Id {id} not found." });
        });

        // POST /products
        app.MapPost("/products", (ProductRequest req) =>
        {
            var product = new ProductADO
            {
                Id = Guid.NewGuid(),
                FamilyId = req.FamilyId,
                Code = req.Code,
                Name = req.Name,
                Price = req.Price,
                Discount = req.Discount
            };

            product.Insert(dbConn);
            return Results.Created($"/products/{product.Id}", product);
        });
    }
}

public record ProductRequest(Guid FamilyId, string Code, string Name, decimal Price, decimal Discount);
