using botiga.Repository;
using botiga.Services;

namespace botiga.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // POST /cart -> crear nou carro
        app.MapPost("/cart", () =>
        {
            var cart = new CartADO { Id = Guid.NewGuid() };
            cart.Create(dbConn);
            return Results.Created($"/cart/{cart.Id}", cart);
        });

        // POST /cart/{cartId}/add -> afegir producte
        app.MapPost("/cart/{cartId}/add", (Guid cartId, CartItemRequest req) =>
        {
            var item = new CartItemADO
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = req.ProductId,
                Quantity = req.Quantity
            };
            item.Add(dbConn);
            return Results.Ok(item);
        });

        // DELETE /cart/{cartId}/remove/{productId} -> treure producte
        app.MapDelete("/cart/{cartId}/remove/{productId}", (Guid cartId, Guid productId) =>
        {
            CartItemADO.Remove(dbConn, cartId, productId);
            return Results.NoContent();
        });
    }
}

public record CartItemRequest(Guid ProductId, int Quantity);
