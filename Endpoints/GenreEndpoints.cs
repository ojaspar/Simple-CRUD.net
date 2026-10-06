using GameStore.Api.Dtos.genre;
using GameStore.Api.Services;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{

    const string GetGenreEndpointName = "GetGenre";
    const string Error = "Genre not found";


    public static void MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");
        group.MapGet("/", async(IGenreService service) => await service.GetAllASync());


        group.MapGet("/{id}", async(int id, IGenreService service)  =>
        {
            var genre = await service.GetByIdAsync(id);
            return genre is null ? Results.BadRequest(Error) : Results.Ok(new GenreDetailsDto(genre.Id, genre.Name, genre.DateCreated, genre.DateUpdated));
        }).WithName(GetGenreEndpointName);


        group.MapPost("/", async(CreateGenreDto newGenre, IGenreService service) =>
        {
            var createdGenre = await service.CreateAsync(newGenre);

            if (createdGenre is null)
            {
                return Results.Conflict("Genre already exists");
            }

            return Results.CreatedAtRoute(GetGenreEndpointName, new { id = createdGenre.Id }, createdGenre);


        });

        group.MapPut("/{id}", async(int id, UpdateGenreDto updatedGenre, IGenreService service) =>
        {
            var existingGenre = await service.GetByIdAsync(id);

            if(existingGenre is null)
            {
                return Results.NotFound(Error);
            }


            return Results.Ok(new GenreDetailsDto(existingGenre.Id, existingGenre.Name, existingGenre.DateCreated, existingGenre.DateUpdated));
        });


         group.MapDelete("/{id}", async (int id, IGenreService service) =>
        {
            var deleted = await service.DeleteAsync(id);

            return deleted ? Results.NotFound(Error) : Results.NoContent();
        });


    }

}
