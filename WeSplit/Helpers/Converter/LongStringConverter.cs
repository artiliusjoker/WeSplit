using System;
using System.Globalization;
using System.Windows.Data;

namespace WeSplit.Helpers.Converter
{
    class LongStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string longString = (string)value;
            return longString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
