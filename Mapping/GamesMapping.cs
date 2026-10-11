using GameStore.Api.Dtos;
using GameStore.Api.models;

namespace GameStore.Api.Mapping;

public static class GamesMapping
{
    public static GameDetailsDto ToDetailsDto(this Game game) => new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate);
    public static GameSummaryDto ToSummaryDto(this Game game) => new GameSummaryDto(game.Id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate);

    public static Game ToEntity(this CreateGameDto newGame) => new Game
    {
        Name = newGame.Name,
        GenreId = newGame.GenreId,
        Price = newGame.Price,
        ReleaseDate = newGame.ReleaseDate
    };

    public static void UpdateFrom(this Game game, UpdateGameDto updateGame)
    {
        game.Name = updateGame.Name;
        game.GenreId = updateGame.GenreId;
        game.Price = updateGame.Price;
        game.ReleaseDate = updateGame.ReleaseDate;
    }
}
