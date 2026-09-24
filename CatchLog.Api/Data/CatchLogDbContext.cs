using CatchLog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Api.Data;

public class CatchLogDbContext : DbContext
{
    public CatchLogDbContext(DbContextOptions<CatchLogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Angler> Anglers { get; set; }
    public DbSet<Species> Species { get; set; }
    public DbSet<Catch> Catches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Angler>()
            .HasIndex(a => a.Email)
            .IsUnique();

        modelBuilder.Entity<Catch>()
            .HasOne(c => c.Angler)
            .WithMany(a => a.Catches)
            .HasForeignKey(c => c.AnglerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Catch>()
            .HasOne(c => c.Species)
            .WithMany(s => s.Catches)
            .HasForeignKey(c => c.SpeciesId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}