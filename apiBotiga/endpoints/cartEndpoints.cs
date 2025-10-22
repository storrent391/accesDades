using botiga.Repository;
using botiga.Services;
using botiga.Validators;
using botiga.Common;

namespace botiga.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // POST /cart -> crear nou carro
        app.MapPost("/cart", (CartRequest req) =>
        {
            Result result = CartValidator.Validate(req);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

            var cart = new CartADO
            {
                Id = Guid.NewGuid(),
                UserId = req.UserId
            };
            cart.Create(dbConn);
            return Results.Created($"/cart/{cart.Id}", cart);
        });

        // POST /cart/add -> afegir producte
        app.MapPost("/cart/add", (CartItemRequest req) =>
        {
            Result result = CartItemValidator.Validate(req, dbConn);
            if (!result.IsOk)
            {
                return Results.BadRequest(new
                {
                    error = result.ErrorCode,
                    message = result.ErrorMessage
                });
            }

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
