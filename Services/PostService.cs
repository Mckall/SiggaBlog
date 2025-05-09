using SiggaBlog.Data;
using System.Collections.ObjectModel;
using System.Text.Json;
using WebApi.Models;

namespace SiggaBlog.Services;

public class PostService
{

    #region Fields
    private readonly IRepository<Post> _postRepository;
    #endregion

    #region CONSTS
    const string URL_BASE = "https://siggablogapptest-h4g8bqg2emd0ajhy.eastus-01.azurewebsites.net";
    #endregion

    #region Constructors
    public PostService(IRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }
    #endregion

    #region Public Methods
    public async Task<List<Post>> GetPosts()
    {
        string _urlGetPosts = $"{URL_BASE}/posts";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Accept", "*/*");

        try
        {
            var response = await client.GetAsync(_urlGetPosts);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Post>>(jsonContent) ?? new List<Post>();

            return result ?? [];
        }
        catch (Exception)
        {
            return [];
        }
    }

    public async Task<List<Post>> GetLocalCachePosts()
    {
        if (_postRepository is null)
            return new List<Post>();

        var localPosts = await _postRepository.GetAllAsync().ConfigureAwait(false);

        return localPosts ?? [];
    }

    public async Task AddUpdateLocalCachePosts(ObservableCollection<Post> posts)
    {
        if (_postRepository is null) return;

        await _postRepository.AddOrUpdateListAsync(posts).ConfigureAwait(false);
    }
    #endregion Public Methods
}