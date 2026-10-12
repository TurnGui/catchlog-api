using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CatchLog.Tests;

public static class TestCacheFactory
{
    public static IDistributedCache Create()
    {
        return new MemoryDistributedCache(
            Options.Create(new MemoryDistributedCacheOptions()));
    }
}