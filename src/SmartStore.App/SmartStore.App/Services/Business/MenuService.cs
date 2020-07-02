using System.Collections.Generic;
using System.Threading.Tasks;
using SmartStore.App.Abstractions.Business;
using SmartStore.App.Models;
using SmartStore.App.ViewModels.Inventory;
using SmartStore.App.ViewModels.Management;
using SmartStore.App.ViewModels.Order;
using SmartStore.App.ViewModels.Report;
using SmartStore.App.ViewModels.Setting;
using SmartStore.App.ViewModels.Terminal;

namespace SmartStore.App.Services.Business
{
    public class MenuService : IMenuService
    {
        public async Task<IEnumerable<MenuModel>> GetMenuListAsync()
        {
            await Task.Delay(1000);

            return new List<MenuModel>
            {
                new MenuModel
                {
                    Title = "Terminal",
                    ImageUrl = "https://img.icons8.com/office/80/000000/pos-terminal.png",
                    ViewModelType = typeof(TerminalViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Orders",
                    ImageUrl = "https://img.icons8.com/color/96/000000/purchase-order.png",
                    ViewModelType = typeof(OrdersViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Inventory",
                    ImageUrl = "https://img.icons8.com/color/96/000000/product.png",
                    ViewModelType = typeof(InventoriesViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Managements",
                    ImageUrl = "https://img.icons8.com/color/96/000000/product-documents.png",
                    ViewModelType = typeof(ManagementsViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Reports",
                    ImageUrl = "https://img.icons8.com/color/96/000000/business-report.png",
                    ViewModelType = typeof(ReportsViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Settings",
                    ImageUrl = "https://img.icons8.com/color/96/000000/automatic.png",
                    ViewModelType = typeof(SettingsViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Help",
                    ImageUrl = "https://img.icons8.com/color/96/000000/questions.png",
                    IsEnabled = false
                },
                new MenuModel
                {
                    Title = "Logout",
                    ImageUrl = "https://img.icons8.com/office/80/000000/export.png",
                    IsEnabled = true
                }
            };
        }

        public async Task<IEnumerable<MenuModel>> GetManagementListAsync()
        {
            await Task.Delay(1000);

            return new List<MenuModel>
            {
                new MenuModel
                {
                    Title = "Products",
                    ImageUrl = "https://img.icons8.com/color/48/000000/product.png",
                    ViewModelType = typeof(ProductsViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Categories",
                    ImageUrl = "https://img.icons8.com/color/48/000000/opened-folder.png",
                    ViewModelType = typeof(CategoriesViewModel),
                    IsEnabled = true
                }
                ,
                new MenuModel
                {
                    Title = "Units",
                    ImageUrl = "https://img.icons8.com/color/48/000000/puzzle.png",
                    ViewModelType = typeof(UnitsViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Customers",
                    ImageUrl = "https://img.icons8.com/color/48/000000/group.png",
                    ViewModelType = typeof(CustomersViewModel),
                    IsEnabled = true
                },
                new MenuModel
                {
                    Title = "Suppliers",
                    ImageUrl = "https://img.icons8.com/color/48/000000/supplier.png",
                    ViewModelType = typeof(SuppliersViewModel),
                    IsEnabled = true
                }
            };
        }
    }
}
