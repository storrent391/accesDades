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

        app.MapPut("/products/{id}", (Guid id, ProductRequest req) =>
        {
            Product existing = ProductADO.GetById(dbConn, id);
            if (existing == null)
            {
                return Results.NotFound();
            }

            Product updated = new Product

            {
                Id = id,
                Code = req.Code,
                Name = req.Name,
                Price = req.Price
            };

            ProductADO.Update(dbConn, updated);

            return Results.Ok(updated);
        }

        );
        // DELETE /products/{id}
        app.MapDelete("/products/{id}", (Guid id) => ProductADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());

    }
    
}

public record ProductRequest(Guid FamilyId, string Code, string Name, decimal Price, decimal Discount);
