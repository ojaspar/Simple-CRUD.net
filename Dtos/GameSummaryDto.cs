namespace GameStore.Api.Dtos;

public record GameSummaryDto(
    int Id, 
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate
);

