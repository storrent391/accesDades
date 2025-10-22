using botiga.Repository;
using botiga.Services;
using botiga.Validators;
using botiga.Common;

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
            Result result = ProductValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new 
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            var product = new ProductADO
            {
                Id = Guid.NewGuid(),
                FamilyId = req.FamilyId,
                Code = req.Code,
                Name = req.Name,
                Price = req.Price,
                Discount = req.Discount
            };

            ProductADO.Insert(dbConn, product);
            return Results.Created($"/products/{product.Id}", product);
        });

        // PUT /products/{id}
        app.MapPut("/products/{id}", (Guid id, ProductRequest req) =>
        {
            Result result = ProductValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new 
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            var existing = ProductADO.GetById(dbConn, id);
            if (existing == null)
                return Results.NotFound();

            existing.Code = req.Code;
            existing.Name = req.Name;
            existing.Price = req.Price;
            existing.Discount = req.Discount;

            ProductADO.Update(dbConn, existing);
            return Results.Ok(existing);
        });

        // DELETE /products/{id}
        app.MapDelete("/products/{id}", (Guid id) =>
        {
            var deleted = ProductADO.Delete(dbConn, id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}

public record ProductRequest(Guid FamilyId, string Code, string Name, decimal Price, decimal Discount);
