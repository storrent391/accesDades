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

            family.Insert(dbConn);
            return Results.Created($"/families/{family.Id}", family);
        });

        app.MapPut("/families/{id}", (Guid id, FamilyRequest req) =>
        {
            Family existing = FamilyADO.GetById(dbConn, id);

            if (existing == null)
            {
                return Results.NotFound();
            }

            Family updated = new Family

            {
                Id = id,
                Name = req.Name,
            };
        }

        );

        app.MapDelete("/families/{id}", (Guid id) => FamilyADO.Delete(dbConn, id) ? Results.NoContent() : Results.NotFound());
    }
}

public record FamilyRequest(string Name);
