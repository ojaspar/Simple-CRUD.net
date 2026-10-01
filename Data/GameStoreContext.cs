using GameStore.Api.models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options): DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genre => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}
