using Microsoft.EntityFrameworkCore;

namespace RokomariScrap.Utils;

public sealed class DatabaseUtils : IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _appDbContext;

    public DatabaseUtils(string connectionString)
    {
        _appDbContext = new AppDbContext(connectionString);
    }

    public async Task Reset()
    {
        await _appDbContext.Database.EnsureDeletedAsync();
        await _appDbContext.Database.EnsureCreatedAsync();
    }


    public async Task RunMigration()
    {
        var migrations = await _appDbContext.Database.GetAppliedMigrationsAsync();
        if (!migrations.Any()) await _appDbContext.Database.MigrateAsync();
    }

    public void Dispose() => _appDbContext.Dispose();

    public async ValueTask DisposeAsync() => await _appDbContext.DisposeAsync();
}