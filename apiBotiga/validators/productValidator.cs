using botiga.Common;
using botiga.Model;
using botiga.Endpoints;
namespace botiga.Validators;

public static class ProductValidator
{
    public static Result Validate(ProductRequest product)
    {
        if (product.FamilyId == Guid.Empty)
        {
            return Result.Failure("El camp FamilyId és obligatori.", "FAMILYID_BUIT");
        }

        if (string.IsNullOrWhiteSpace(product.Code))
        {
            return Result.Failure("El codi del producte és obligatori.", "CODE_BUIT");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return Result.Failure("El nom del producte és obligatori.", "NAME_BUIT");
        }

        if (product.Price <= 0)
        {
            return Result.Failure("El preu ha de ser superior a 0.", "PREU_INCORRECTE");
        }

        if (product.Discount < 0 || product.Discount > 100)
        {
            return Result.Failure("El descompte ha d'estar entre 0 i 100.", "DESCOMPTE_INCORRECTE");
        }

        return Result.Ok();
    }
}
