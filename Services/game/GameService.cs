using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Mapping;
using GameStore.Api.models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Services;

public class GameService(GameStoreContext dbContext) : IGameService
{
    public async Task<IEnumerable<GameSummaryDto>> GetAllAsync()
    {
        List<Game> games = await dbContext.Games
            .Include(g => g.Genre)
            .AsNoTracking()
            .ToListAsync();

        return games.Select(g => g.ToSummaryDto());
    }

    public async Task<GameDetailsDto?> GetByIdAsync(int id)
    {
        Game? game = await dbContext.Games.FindAsync(id);

        return game?.ToDetailsDto();
    }

    public async Task<GameDetailsDto?> CreateAsync(CreateGameDto newGame)
    {
        bool nameExists = await dbContext.Games.AnyAsync(g => g.Name == newGame.Name);

        if (nameExists)
        {
            return null;
        }

        Game game = newGame.ToEntity();

        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        return game.ToDetailsDto();
    }

    public async Task<bool> UpdateAsync(int id, UpdateGameDto updateGame)
    {
        Game? game = await dbContext.Games.FindAsync(id);
        if (game is null)
        {
            return false;
        }

        game.UpdateFrom(updateGame);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int deletedRows = await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }
}
