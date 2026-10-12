using CatchLog.Api.DTOs;
using CatchLog.Api.Exceptions;
using CatchLog.Api.Models;
using CatchLog.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Tests;

public class AnglerServiceTests
{
    [Fact]
    public async Task CreateAsync_ValidDto_StoresHashedPassword()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var service = new AnglerService(context);
        var dto = new CreateAnglerDto
        {
            Name = "Ana",
            Email = "ana@test.com",
            Password = "test1234"
        };

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        var stored = await context.Anglers.SingleAsync(a => a.Id == result.Id);
        Assert.NotEqual("test1234", stored.PasswordHash);
        Assert.True(BCrypt.Net.BCrypt.Verify("test1234", stored.PasswordHash));
    }

    [Fact]
    public async Task UpdateAsync_EmailUsedByAnotherAngler_ThrowsConflictException()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var ana = new Angler { Name = "Ana", Email = "ana@test.com" };
        var bob = new Angler { Name = "Bob", Email = "bob@test.com" };
        context.Anglers.AddRange(ana, bob);
        await context.SaveChangesAsync();

        var service = new AnglerService(context);
        var dto = new UpdateAnglerDto { Name = "Ana", Email = "bob@test.com" };

        // Act + Assert
        await Assert.ThrowsAsync<ConflictException>(
            () => service.UpdateAsync(ana.Id, dto));
    }

    [Fact]
    public async Task UpdateAsync_KeepingOwnEmail_UpdatesName()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var ana = new Angler { Name = "Ana", Email = "ana@test.com" };
        context.Anglers.Add(ana);
        await context.SaveChangesAsync();

        var service = new AnglerService(context);
        var dto = new UpdateAnglerDto { Name = "Ana Silva", Email = "ana@test.com" };

        // Act
        var result = await service.UpdateAsync(ana.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ana Silva", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_UnknownAngler_ReturnsNull()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();
        var service = new AnglerService(context);
        var dto = new UpdateAnglerDto { Name = "X", Email = "x@test.com" };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        Assert.Null(result);
    }
}