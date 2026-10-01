using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos.genre;

public record CreateGenreDto(
    [Required][StringLength(50)] string Name,
    [Required] int GenreId,
    DateTime DateCreated,
    DateTime DateUpdated
);
