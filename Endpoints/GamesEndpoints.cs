using GameStore.Api.Dtos;
using GameStore.Api.Services;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    const string Error = "Game not found";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        group.MapGet("/", async (IGameService service) => await service.GetAllAsync());


        group.MapGet("/{id}", async (int id, IGameService service) =>
        {
            var game = await service.GetByIdAsync(id);
            return game is null ? Results.BadRequest(Error) : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto newGame, IGameService service) =>
        {
            var createdGame = await service.CreateAsync(newGame);

            if (createdGame is null)
            {
                return Results.Conflict("Game already exists");
            }

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = createdGame.Id }, createdGame);
        });


        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, IGameService service) =>
        {
            var updated = await service.UpdateAsync(id, updateGame);

            return updated ? Results.NoContent() : Results.BadRequest(Error);
        });


        group.MapDelete("/{id}", async (int id, IGameService service) =>
        {
            var deleted = await service.DeleteAsync(id);

            return deleted ? Results.NoContent() : Results.NotFound(Error);
        });

    }

}
