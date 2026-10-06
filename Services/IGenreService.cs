using GameStore.Api.Dtos.genre;

namespace GameStore.Api.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreSummaryDto>> GetAllASync();
    Task<GenreDetailsDto?> GetByIdAsync(int id);
    Task<GenreDetailsDto> CreateAsync(CreateGenreDto newGenre);

    Task<bool> UpdateAsync(int id, UpdateGenreDto updateGenre);
    Task<bool> DeleteAsync(int id);
}
