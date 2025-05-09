using CommunityToolkit.Mvvm.Input;
using SiggaBlog.Data;
using SiggaBlog.Services;
using System.Collections.ObjectModel;
using WebApi.Models;

namespace SiggaBlog.ViewModel;

public class MainPageViewModel : BaseViewModel
{

    #region Fields
    private readonly PostService _postService;
    #endregion

    #region Properties
    public ObservableCollection<Post> Posts { get; set; } = new ObservableCollection<Post>();

    private bool _isBusy = false;
    public bool IsBusy
    {
        get { return _isBusy; }
        set
        {
            if (_isBusy == value) return;
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public MainPageViewModel(IRepository<Post> postsRepository)
    {
        _postService = new PostService(postsRepository);
        _ = this.GetPosts();
    }

    private async Task GetPosts()
    {
        IsBusy = true;

        if (_postService is not null)
        {
            
            var posts = await _postService.GetPosts();
            if (posts is not null)
            {
                foreach (var post in posts)
                {
                    Posts.Add(post);
                }
            }

            await this.SaveLocalCache().ConfigureAwait(false);

        }

        IsBusy = false;
    }

    private async Task SaveLocalCache()
    {
        if (_postService is null) return;

        await _postService.AddUpdateLocalCachePosts(this.Posts).ConfigureAwait(false);
    }
}