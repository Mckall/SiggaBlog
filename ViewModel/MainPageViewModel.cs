using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MainApp.Input;
using SiggaBlog.Commons.Permissions;
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
    private ObservableCollection<Post> _posts = new();
    public ObservableCollection<Post> Posts
    {
        get { return _posts; }
        set
        {
            if (_posts == value) return;
            _posts = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsBusy));
        }
    }


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

    #region Commands
    public ObservableCommand? RefreshCommand { get; set; }
    #endregion

    #region Constructors
    public MainPageViewModel(IRepository<Post> postsRepository)
    {
        _postService = new PostService(postsRepository);
        RefreshCommand = new ObservableCommand(OnRefresh);
        _ = this.GetPosts();
    }

    private void OnRefresh(object arg1, PermissionArgs args)
    {
        _ = GetPosts();
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

        foreach (var post in posts)
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