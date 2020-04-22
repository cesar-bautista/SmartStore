using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace SmartStore.App.Converters
{
    /// <summary>
    /// Converter for using in Entry fields for masked input of currency.
    /// The binded property must be of type decimal, and must invoke the
    /// PropertyChangedEventArgs event whenever the value is changed, so that the desired mask behavior is kept.
    /// </summary>
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return decimal.Parse(value.ToString()).ToString("C");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueFromString = Regex.Replace(value.ToString(), @"\D", "");
            
            if (valueFromString.Length <= 0)
                return 0m;
            
            if (!long.TryParse(valueFromString, out var valueLong))
                return 0m;

            if (valueLong <= 0)
                return 0m;

            return valueLong / 100m;
        }
    }
}
