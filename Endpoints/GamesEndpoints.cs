using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.models;

namespace GameStore.Api.Endpoints;




public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    const string Error = "Game not found";
 
    private static readonly List<GameDto> games = [
        new (1, "Game 1", "Genre 1", 19.99m, DateOnly.FromDateTime(DateTime.Now)),
        new (2, "Game 2", "Genre 2", 29.99m, DateOnly.FromDateTime(DateTime.Now)),
        new (3, "Game 3", "Genre 3", 39.9m, DateOnly.FromDateTime(DateTime.Now))
    ];
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        group.MapGet("/", () => games);
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(g => g.Id == id);
            return game is null ? Results.BadRequest(Error) : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext DbContext) =>
        {

            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate

            };
            // if (string.IsNullOrEmpty(newGame.Name))
            // {
            //     return Results.BadRequest("Name is required");
            // }
            // if (string.IsNullOrEmpty(newGame.Genre))
            // {
            //     return Results.BadRequest("Genre is required");
            // }

            // if (decimal.IsNegative(newGame.Price))
            // {
            //     return Results.BadRequest("Enter a valid Price");
            // }

            // var validGame =  games.Find(g => g.Name == newGame.Name);
            // if(validGame is not null)
            // {
            //     return Results.BadRequest("Game already exist");
            // }
            DbContext.Games.Add(game);
            DbContext.SaveChanges();
            
            GameDetailsDto gameDto = new(
                game.Id, game.Name, game.GenreId,game.Price, game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);

        });


        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index < 0)
            {
                return Results.BadRequest(Error);
            }
            games[index] = new GameDto(id, updateGame.Name, updateGame.Genre, updateGame.Price, DateOnly.FromDateTime(updateGame.ReleaseDate));
            return Results.NoContent();
        });


        group.MapDelete("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            Console.WriteLine(games);

            if (game is null)
            {
                return Results.BadRequest(Error);
            } 
            games.Remove(game);
            return  Results.NoContent();
        });

    }

}
