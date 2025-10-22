using botiga.Common;
using botiga.Model;
using botiga.Endpoints;
namespace botiga.Validators;

public static class CartValidator
{
    public static Result Validate(CartRequest cart)
    {
        if (cart.UserId == Guid.Empty)
            return Result.Failure("El camp UserId és obligatori.", "USERID_BUIT");

        return Result.Ok();
    }
}
