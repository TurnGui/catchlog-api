using CatchLog.Api.DTOs;
using CatchLog.Api.Exceptions;
using CatchLog.Api.Models;
using CatchLog.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Tests;

public class CatchServiceTests
{
    [Fact]
    public async Task CreateAsync_ProtectedSpeciesWithoutCatchAndRelease_ThrowsBusinessRuleException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var angler = new Angler { Name = "Ana", Email = "ana@test.com" };
        var species = new Species
        {
            CommonName = "Atlantic Salmon",
            ScientificName = "Salmo salar",
            IsProtected = true
        };
        context.Anglers.Add(angler);
        context.Species.Add(species);
        await context.SaveChangesAsync();

        var service = new CatchService(context);
        var dto = new CreateCatchDto
        {
            SpeciesId = species.Id,
            WeightKg = 4,
            LengthCm = 70,
            Location = "Rio Minho",
            CaughtAt = DateTime.UtcNow,
            IsCatchAndRelease = false
        };

        // Act + Assert
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => service.CreateAsync(angler.Id, dto));
        Assert.Equal(0, await context.Catches.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_ProtectedSpeciesWithCatchAndRelease_SavesCatch()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var angler = new Angler { Name = "Ana", Email = "ana@test.com" };
        var species = new Species
        {
            CommonName = "Atlantic Salmon",
            ScientificName = "Salmo salar",
            IsProtected = true
        };
        context.Anglers.Add(angler);
        context.Species.Add(species);
        await context.SaveChangesAsync();

        var service = new CatchService(context);
        var dto = new CreateCatchDto
        {
            SpeciesId = species.Id,
            WeightKg = 4,
            LengthCm = 70,
            Location = "Rio Minho",
            CaughtAt = DateTime.UtcNow,
            IsCatchAndRelease = true
        };

        // Act
        var result = await service.CreateAsync(angler.Id, dto);

        // Assert
        Assert.Equal(angler.Id, result.AnglerId);
        Assert.Equal("Atlantic Salmon", result.SpeciesCommonName);
        Assert.Equal(1, await context.Catches.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_UnknownAngler_ThrowsNotFoundException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var species = new Species { CommonName = "Sea Bass", ScientificName = "Dicentrarchus labrax" };
        context.Species.Add(species);
        await context.SaveChangesAsync();

        var service = new CatchService(context);
        var dto = new CreateCatchDto
        {
            SpeciesId = species.Id,
            Location = "Rio Cávado",
            CaughtAt = DateTime.UtcNow
        };

        // Act + Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.CreateAsync(999, dto));
    }

    [Fact]
    public async Task DeleteAsync_CatchOfAnotherAngler_ThrowsForbiddenException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var owner = new Angler { Name = "Ana", Email = "ana@test.com" };
        var other = new Angler { Name = "Bob", Email = "bob@test.com" };
        var species = new Species { CommonName = "Sea Bass", ScientificName = "Dicentrarchus labrax" };
        context.Anglers.AddRange(owner, other);
        context.Species.Add(species);
        await context.SaveChangesAsync();

        var fishCatch = new Catch
        {
            AnglerId = owner.Id,
            SpeciesId = species.Id,
            Location = "Rio Cávado",
            CaughtAt = DateTime.UtcNow
        };
        context.Catches.Add(fishCatch);
        await context.SaveChangesAsync();

        var service = new CatchService(context);

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenException>(
            () => service.DeleteAsync(fishCatch.Id, other.Id));
        Assert.Equal(1, await context.Catches.CountAsync());
    }
}