using botiga.Common;
using botiga.Repository;
using botiga.Services;
using botiga.Model;
using botiga.Endpoints;
namespace botiga.Validators;

public static class FamilyValidator
{
    public static Result Validate(FamilyRequest family, DatabaseConnection dbConn, bool isUpdate = false, Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(family.Name))
            return Result.Failure("El nom de la família és obligatori.", "NOM_BUIT");

        var families = FamilyADO.GetAll(dbConn);
        bool exists = families.Any(f =>
            f.Name.Equals(family.Name, StringComparison.OrdinalIgnoreCase) &&
            (!isUpdate || f.Id != id)
        );

        if (exists)
            return Result.Failure($"Ja existeix una família amb el nom '{family.Name}'.", "FAMILIA_DUPLICADA");

        return Result.Ok();
    }
}
