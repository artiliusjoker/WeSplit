using System;
using System.Globalization;
using System.Windows.Data;

namespace WeSplit.Helpers.Converter
{
    class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double @double && @double == 0)
            {
                return "0 VND";
            }
            return String.Format("{0:#,#} VND", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
