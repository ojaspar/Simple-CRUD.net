using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;


public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    const string Error = "Game not found";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.Select(
            game => new GameSummaryDto(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate

            )
        ).AsNoTracking().ToListAsync()
        );


        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is null ? Results.BadRequest(Error) : Results.Ok(new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate));
        }).WithName(GetGameEndpointName);

        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext DbContext) =>
        {

            if (await DbContext.Games.AnyAsync(g => g.Name == newGame.Name))
            {
                return Results.Conflict("Game already exists");
            }

            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate

            };
            DbContext.Games.Add(game);
            await DbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);

        });


        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);


            if (existingGame is null)
            {
                return Results.BadRequest(Error);
            }

            existingGame.Name = updateGame.Name;
            existingGame.Price = updateGame.Price;
            existingGame.GenreId = updateGame.GenreId;
            existingGame.ReleaseDate = updateGame.ReleaseDate;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });


        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var deleted = await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();

            return deleted == 0 ? Results.NotFound(Error) : Results.NoContent();
        });

    }

}
