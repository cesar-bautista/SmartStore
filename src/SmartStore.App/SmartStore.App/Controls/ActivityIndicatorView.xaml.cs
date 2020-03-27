using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartStore.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityIndicatorView : Grid
    {
        public ActivityIndicatorView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsRunningProperty = 
            BindableProperty.Create(
                nameof(IsRunning), 
                typeof(bool), 
                typeof(ActivityIndicatorView),
                null);
        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }
    }
}