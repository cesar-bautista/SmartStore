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
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(TerminalViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Orders",
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(OrderViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Inventory",
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(InventoryViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Managements",
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(ManagementViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Reports",
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(ReportViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Settings",
                    ImageUrl = string.Empty,
                    ViewModelType = typeof(SettingViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Help",
                    ImageUrl = string.Empty,
                    IsEnabled = false
                }
            };
        }
    }
}
