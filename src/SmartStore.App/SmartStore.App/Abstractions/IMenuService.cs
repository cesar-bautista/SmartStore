using System.Collections.ObjectModel;
using SmartStore.App.Models;

namespace SmartStore.App.Abstractions
{
    public interface IMenuService
    {
        ObservableCollection<MenuItemModel> GetMenuAsync();
    }
}
