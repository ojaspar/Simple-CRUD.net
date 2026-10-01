using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos.genre;
using GameStore.Api.models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{

    const string GetGenreEndpointName = "GetGenre";
    const string Error = "Genre not found";


    public static void MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");
        group.MapGet("/", async(GameStoreContext dbContext) => await dbContext.Genre.Select(
            genre => new GenreSummaryDto(
                genre.Id, genre.Name
            )
        ).AsNoTracking().ToListAsync()
        );

        group.MapGet("/{id}", async(int id, GameStoreContext DbContext)  =>
        {
            var genre = await DbContext.Genre.FindAsync(id);
            return genre is null ? Results.BadRequest(Error) : Results.Ok(new GenreDetailsDto(genre.Id, genre.Name, genre.DateCreated, genre.DateUpdated));
        }).WithName(GetGenreEndpointName);


        group.MapPost("/", async(CreateGenreDto newGenre, GameStoreContext DbContext) =>
        {
            if(await DbContext.Genre.AnyAsync(g => g.Name == newGenre.Name))
            {
                return Results.Conflict("Genre already exists");
            }

            Genre genre = new()
            {
               Name = newGenre.Name,
               DateCreated = DateTime.UtcNow, 
               DateUpdated = DateTime.UtcNow
            };

            DbContext.Genre.Add(genre);
            await DbContext.SaveChangesAsync();

            GenreDetailsDto genreDetailsDto = new(genre.Id, genre.Name, genre.DateCreated, genre.DateUpdated);
            return Results.CreatedAtRoute(GetGenreEndpointName, new { id = genre.Id }, genre);


        });

        group.MapPut("/{id}", async(int id, UpdateGenreDto updatedGenre, GameStoreContext DbContext) =>
        {
            var existingGenre = await DbContext.Genre.FindAsync(id);

            if(existingGenre is null)
            {
                return Results.NotFound(Error);
            }

            existingGenre.Name = updatedGenre.Name;
            existingGenre.DateUpdated =  DateTime.UtcNow;

            await DbContext.SaveChangesAsync();
            return Results.Ok(new GenreDetailsDto(existingGenre.Id, existingGenre.Name, existingGenre.DateCreated, existingGenre.DateUpdated));
        });


         group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var deleted = await dbContext.Genre
                .Where(genre => genre.Id == id)
                .ExecuteDeleteAsync();

            return deleted == 0 ? Results.NotFound(Error) : Results.NoContent();
        });


    }

}
