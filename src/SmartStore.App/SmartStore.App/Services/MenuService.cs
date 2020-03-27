using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SmartStore.App.Abstractions;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Inventory;
using SmartStore.App.ViewModels.Management;
using SmartStore.App.ViewModels.Order;
using SmartStore.App.ViewModels.Report;
using SmartStore.App.ViewModels.Setting;
using SmartStore.App.ViewModels.Terminal;

namespace SmartStore.App.Services
{
    public class MenuService : IMenuService
    {
        public async Task<ObservableCollection<MenuItemModel>> GetListAsync()
        {
            await Task.Delay(1000);

            return new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel
                {
                    Title = "Terminal",
                    ImageUrl = "https://img.icons8.com/office/80/000000/pos-terminal.png",
                    ViewModelType = typeof(TerminalViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Orders",
                    ImageUrl = "https://img.icons8.com/color/96/000000/purchase-order.png",
                    ViewModelType = typeof(OrderViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Inventory",
                    ImageUrl = "https://img.icons8.com/color/96/000000/product.png",
                    ViewModelType = typeof(InventoryViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Managements",
                    ImageUrl = "https://img.icons8.com/color/96/000000/product-documents.png",
                    ViewModelType = typeof(ManagementViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Reports",
                    ImageUrl = "https://img.icons8.com/color/96/000000/business-report.png",
                    ViewModelType = typeof(ReportViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Settings",
                    ImageUrl = "https://img.icons8.com/color/96/000000/automatic.png",
                    ViewModelType = typeof(SettingViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Help",
                    ImageUrl = "https://img.icons8.com/color/96/000000/questions.png",
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Logout",
                    ImageUrl = "https://img.icons8.com/office/80/000000/export.png",
                    IsEnabled = true
                }
            };
        }
    }
}
