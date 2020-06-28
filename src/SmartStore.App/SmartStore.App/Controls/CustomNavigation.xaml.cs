using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartStore.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigation : NavigationPage
    {
        public CustomNavigation() : base()
        {
            InitializeComponent();
        }

        public CustomNavigation(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}