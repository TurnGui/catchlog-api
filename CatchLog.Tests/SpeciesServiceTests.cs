using CatchLog.Api.Exceptions;
using CatchLog.Api.Models;
using CatchLog.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Tests;

public class SpeciesServiceTests
{
    [Fact]
    public async Task DeleteAsync_SpeciesWithCatches_ThrowsConflictException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var angler = new Angler { Name = "Ana", Email = "ana@test.com" };
        var species = new Species { CommonName = "Sea Bass", ScientificName = "Dicentrarchus labrax" };
        context.Anglers.Add(angler);
        context.Species.Add(species);
        await context.SaveChangesAsync();

        context.Catches.Add(new Catch
        {
            AnglerId = angler.Id,
            SpeciesId = species.Id,
            Location = "Rio Cávado",
            CaughtAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var service = new SpeciesService(context);

        // Act + Assert
        await Assert.ThrowsAsync<ConflictException>(
            () => service.DeleteAsync(species.Id));
        Assert.Equal(1, await context.Species.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_SpeciesWithoutCatches_RemovesSpecies()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var species = new Species { CommonName = "Sea Bass", ScientificName = "Dicentrarchus labrax" };
        context.Species.Add(species);
        await context.SaveChangesAsync();

        var service = new SpeciesService(context);

        // Act
        var deleted = await service.DeleteAsync(species.Id);

        // Assert
        Assert.True(deleted);
        Assert.Equal(0, await context.Species.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_UnknownSpecies_ReturnsFalse()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var service = new SpeciesService(context);

        // Act
        var deleted = await service.DeleteAsync(999);

        // Assert
        Assert.False(deleted);
    }
}