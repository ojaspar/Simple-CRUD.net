
using GameStore.Api.Dtos.genre;
using GameStore.Api.models;

namespace GameStore.Api.Mapping;

public static class GenreMapping
{
    public static GenreDetailsDto ToDetailsDto (this Genre genre) => new GenreDetailsDto(genre.Id, genre.Name, genre.DateCreated, genre.DateUpdated);
    public static GenreSummaryDto ToSummaryDto (this Genre genre) => new GenreSummaryDto(genre.Id, genre.Name);
}
