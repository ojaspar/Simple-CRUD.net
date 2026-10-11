using GameStore.Api.Dtos;

namespace GameStore.Api.Services;

public interface IGameService
{
    Task<IEnumerable<GameSummaryDto>> GetAllAsync();
    Task<GameDetailsDto?> GetByIdAsync(int id);
    Task<GameDetailsDto?> CreateAsync(CreateGameDto newGame);

    Task<bool> UpdateAsync(int id, UpdateGameDto updateGame);
    Task<bool> DeleteAsync(int id);
}
