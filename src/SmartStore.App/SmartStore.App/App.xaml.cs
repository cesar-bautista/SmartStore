using System.Threading.Tasks;
using SmartStore.App.Abstractions.Ui;
using SmartStore.App.ViewModels.Base;
using Xamarin.Forms;

namespace SmartStore.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            InitNavigation();
        }

        private Task InitNavigation()
        {
            var navigationService = LocatorViewModel.Instance.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
