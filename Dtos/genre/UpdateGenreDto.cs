using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos.genre;

public record UpdateGenreDto(
    [Required][StringLength(50)] string Name,
    [Required] int GenreId,
    DateTime DateUpdated
);
