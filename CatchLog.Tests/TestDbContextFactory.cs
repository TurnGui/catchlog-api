using CatchLog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CatchLog.Tests;

public static class TestDbContextFactory
{
    public static CatchLogDbContext Create()
    {
        var options = new DbContextOptionsBuilder<CatchLogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CatchLogDbContext(options);
    }
}