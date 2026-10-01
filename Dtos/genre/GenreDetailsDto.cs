namespace GameStore.Api.Dtos.genre;

public record GenreDetailsDto(
    int Id,
    string Name,
    DateTime DateCreated,
    DateTime DateUpdated
);
