using botiga.Common;
using botiga.Repository;
using botiga.Services;
using botiga.Model;
using botiga.Endpoints;
namespace botiga.Validators;

public static class CartItemValidator
{
    public static Result Validate(CartItemRequest item, DatabaseConnection dbConn)
    {
        if (item.CartId == Guid.Empty)
            return Result.Failure("El camp CartId és obligatori.", "CARTID_BUIT");

        if (item.ProductId == Guid.Empty)
            return Result.Failure("El camp ProductId és obligatori.", "PRODUCTID_BUIT");

        if (item.Quantity <= 0)
            return Result.Failure("La quantitat ha de ser superior a 0.", "QUANTITAT_INCORRECTE");

        // Comprovar que el producte existeix
        var product = ProductADO.GetById(dbConn, item.ProductId);
        if (product is null)
            return Result.Failure("El producte especificat no existeix.", "PRODUCTE_INEXISTENT");

        return Result.Ok();
    }
}
