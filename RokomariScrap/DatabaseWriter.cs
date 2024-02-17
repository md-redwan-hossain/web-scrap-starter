namespace RokomariScrap;

public sealed class DatabaseWriter : IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _appDbContext;
    private readonly Queue<Book> _queue;

    public DatabaseWriter(AppDbContext appDbContext, Queue<Book> queue)
    {
        _appDbContext = appDbContext;
        _queue = queue;
    }

    public async Task Write()
    {
        while (_queue.Count != 0)
        {
            var item = _queue.Dequeue();

            await _appDbContext.Books.AddAsync(item);
            await _appDbContext.SaveChangesAsync();
        }
    }

    public void Dispose() => _appDbContext.Dispose();
    public async ValueTask DisposeAsync() => await _appDbContext.DisposeAsync();
}