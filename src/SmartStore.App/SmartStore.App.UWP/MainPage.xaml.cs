namespace SmartStore.App.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new SmartStore.App.App());
        }
    }
}
