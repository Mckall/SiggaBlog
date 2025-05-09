using SQLite;
using WebApi.Models;

namespace SiggaBlog.Data;

public class PostsRepository : IRepository<Post>
{
    private readonly SQLiteAsyncConnection _db;

    public PostsRepository(AppDbContext context)
    {
        _db = context.Connection;
    }

    public async Task<List<Post>> GetAllAsync()
    {
        return await _db.Table<Post>().ToListAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        return await _db.FindAsync<Post>(id);
    }

    public async Task AddAsync(Post item)
    {
        await _db.InsertAsync(item);
    }

    public async Task AddAllAsync(IEnumerable<Post> posts)
    {
        await _db.InsertAllAsync(posts);
    }

    public async Task UpdateAsync(Post item)
    {
        await _db.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetByIdAsync(id);
        if (item != null)
            await _db.DeleteAsync(item);
    }

    public async Task AddOrUpdateListAsync(IEnumerable<Post> posts)
    {
        var existsItems = await _db.Table<Post>().ToListAsync();

        if (existsItems.Any())
        {
            foreach (var post in posts)
            {
                var existente = existsItems.FirstOrDefault(p => p.Id == post.Id);
                if (existente != null)
                {
                    await _db.UpdateAsync(post);
                }
                else
                {
                    await _db.InsertAsync(post);
                }
            }
        }
        else
        {
            await _db.InsertAllAsync(posts);
        }
    }
}
