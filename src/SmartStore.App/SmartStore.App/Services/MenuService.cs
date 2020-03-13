using System.Collections.ObjectModel;
using SmartStore.App.Abstractions;
using SmartStore.App.Enums;
using SmartStore.App.Models;
using SmartStore.App.ViewModels;

namespace SmartStore.App.Services
{
    public class MenuService : IMenuService
    {
        public ObservableCollection<MenuItemModel> GetMenuAsync()
        {
            return new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel
                {
                    Title = "POS Terminal",
                    MenuItemType = MenuItemType.Home,
                    ViewModelType = typeof(HomeViewModel),
                    IsEnabled = true
                },
                new MenuItemModel
                {
                    Title = "Stock In",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Managements",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Inventory",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Reports",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Settings",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                },
                new MenuItemModel
                {
                    Title = "Help",
                    MenuItemType = MenuItemType.Settings,
                    IsEnabled = false
                }
            };
        }
    }
}
