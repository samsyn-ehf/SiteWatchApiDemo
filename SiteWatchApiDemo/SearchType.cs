using System;
using System.Globalization;
using System.Windows.Data;

namespace SiteWatchApiDemo
{
    public class SearchType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int c)
            {
                return SiteWatchWebApiModel.Search.SearchItem.ConvertCtoString(c);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
