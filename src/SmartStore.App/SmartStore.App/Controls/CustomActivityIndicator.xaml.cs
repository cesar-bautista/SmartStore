using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartStore.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomActivityIndicator : Grid
    {
        public CustomActivityIndicator()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsRunningProperty = 
            BindableProperty.Create(
                nameof(IsRunning), 
                typeof(bool), 
                typeof(CustomActivityIndicator),
                null);
        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }
    }
}