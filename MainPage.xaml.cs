using SiggaBlog.Data;
using SiggaBlog.ViewModel;
using WebApi.Models;

namespace SiggaBlog
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
