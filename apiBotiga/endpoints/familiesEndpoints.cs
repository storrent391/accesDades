using botiga.Repository;
using botiga.Services;
using botiga.Model;

namespace botiga.Endpoints;

public static class FamilyEndpoints
{
    public static void MapFamilyEndpoints(this WebApplication app, DatabaseConnection dbConn)
    {
        // GET /families
        app.MapGet("/families", () =>
        {
            var families = FamilyADO.GetAll(dbConn);
            return Results.Ok(families);
        });

        // POST /families
        app.MapPost("/families", (FamilyRequest req) =>
        {
            var family = new FamilyADO
            {
                Id = Guid.NewGuid(),
                Name = req.Name
            };

            FamilyADO.Insert(dbConn, family);
            return Results.Created($"/families/{family.Id}", family);
        });

        // PUT /families/{id}
        app.MapPut("/families/{id}", (Guid id, FamilyRequest req) =>
        {
            var existing = FamilyADO.GetById(dbConn, id);

            if (existing == null)
                return Results.NotFound();

            existing.Name = req.Name;
            FamilyADO.Update(dbConn, existing);

            return Results.Ok(existing);
        });

        // DELETE /families/{id}
        app.MapDelete("/families/{id}", (Guid id) =>
        {
            var deleted = FamilyADO.Delete(dbConn, id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}

public record FamilyRequest(string Name);
