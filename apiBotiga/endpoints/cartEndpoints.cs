using botiga.Repository;
using botiga.Services;

namespace botiga.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // POST /cart -> crear nou carro
        app.MapPost("/cart", (CartRequest req) =>
        {
            var cart = new CartADO
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId
            };
            cart.Create(dbConn);
            return Results.Created($"/cart/{cart.Id}", cart);
        });

        // POST /cart/add -> afegir producte
        app.MapPost("/cart/add", (Guid cartId, CartItemRequest req) =>
        {
            var item = new CartItemADO
            {
                Id = Guid.NewGuid(),
                CartId = req.CartId,
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

public record CartItemRequest(Guid ProductId, Guid CartId, int Quantity);
public record CartRequest(Guid UserId);