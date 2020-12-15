using System;
using System.Globalization;
using System.Windows.Data;

namespace WeSplit.Helpers.Converter
{
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imgPath = (string)value;
            if(imgPath == "")
            {
                return "/Assets/sidebar_background.jpg";
            }    
            if(System.IO.Path.IsPathRooted(imgPath))
            {
                return imgPath;
            }    
            imgPath = imgPath.Replace('/' , '\\');
            var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            var absolute = $"{baseFolder}{imgPath}";
            return absolute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
