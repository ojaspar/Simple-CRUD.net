

namespace GameStore.Api.models;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
