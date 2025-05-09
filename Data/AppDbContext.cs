using SQLite;
using WebApi.Models;

namespace SiggaBlog.Data;

public class AppDbContext
{
    private readonly SQLiteAsyncConnection _connection;

    public AppDbContext()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
        _connection = new SQLiteAsyncConnection(dbPath);
        _connection.CreateTableAsync<Post>().Wait();
    }

    public SQLiteAsyncConnection Connection => _connection;
}
