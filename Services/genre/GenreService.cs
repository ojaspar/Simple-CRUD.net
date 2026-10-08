using GameStore.Api.Data;
using GameStore.Api.Dtos.genre;
using GameStore.Api.Mapping;
using GameStore.Api.models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Services;

public class GenreService(GameStoreContext dbContext) : IGenreService

{
    public async Task<GenreDetailsDto> CreateAsync(CreateGenreDto newGenre)
    {
        bool nameExists = await dbContext.Genre.AnyAsync(g => g.Name == newGenre.Name);

        if (nameExists)
        {
            return null;
        }

        DateTime now = DateTime.UtcNow;
        Genre genre = new()
        {
            Name = newGenre.Name,
            DateCreated = now, DateUpdated = now
        };

        dbContext.Genre.Add(genre);
        await dbContext.SaveChangesAsync();
        return genre.ToDetailsDto();
    }

 

    public async Task<IEnumerable<GenreSummaryDto>> GetAllASync()
    {
        
        return await dbContext.Genre.Select(g => new GenreSummaryDto(g.Id, g.Name)).AsTracking().ToListAsync();
    }

    public async Task<GenreDetailsDto?> GetByIdAsync(int id)
    {
        Genre genre = await dbContext.Genre.FindAsync(id);

        return genre?.ToDetailsDto();
    }


   public async Task<bool> DeleteAsync(int id)
    {
        int deletedRows = await dbContext.Genre.Where(g => g.Id == id).ExecuteDeleteAsync();
        return deletedRows > 0;
    }
    public async Task<bool> UpdateAsync( int id, UpdateGenreDto updateGenre)
    {
        Genre? genre = await dbContext.Genre.FindAsync(id);
        if(genre is null)
        {
            return false;
        }

        genre.Name = updateGenre.Name;
        genre.DateUpdated = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
        return true;
    }
}
