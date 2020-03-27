using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartStore.App.Extensions
{
    public static class ObservableExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var collection = new ObservableCollection<T>();

            foreach (var item in source)
            {
                collection.Add(item);
            }

            return collection;
        }
    }
}
