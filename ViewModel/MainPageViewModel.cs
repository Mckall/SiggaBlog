using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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

    #region Constructors
    public MainPageViewModel(IRepository<Post> postsRepository)
    {
        _postService = new PostService(postsRepository);
        _ = this.GetPosts();
    }
    #endregion

    #region Private Methods
    private async Task GetPosts()
    {
        IsBusy = true;

        if (_postService is not null)
        {

            var posts = await _postService.GetPosts();

            if (HasInternetSignal() && posts is not null && posts.Any())
            {
                foreach (var post in posts)
                {
                    Posts.Add(post);
                }

                await this.SaveLocalCache().ConfigureAwait(false);

                await Toast.Make("Lista atualizada.", ToastDuration.Short).Show();
            }
            else
            {
                await this.GetLocalCachePosts();
            }

        }

        IsBusy = false;
    }

    private bool HasInternetSignal()
    {
        return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }

    private async Task GetLocalCachePosts()
    {
        await Toast.Make("Sem conexão com a internet: buscando dados em cache.", ToastDuration.Short).Show();

        if (_postService is null) return;
        Posts = new();

        var posts = await _postService.GetLocalCachePosts();

        foreach(var post in posts)
        {
            Posts.Add(post);
        }

        OnPropertyChanged(nameof(Posts));
    }

    private async Task SaveLocalCache()
    {
        if (_postService is null) return;

        await _postService.AddUpdateLocalCachePosts(this.Posts).ConfigureAwait(false);
    }
    #endregion
}