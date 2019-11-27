using System;
using System.Globalization;
using System.Windows.Data;

namespace ClassicAssist.UI.ValueConverters
{
    public class CellWidthValueConverter : IValueConverter
    {
        private const int substractValue = 15;
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            double? val = (double?) value - substractValue;
            return val;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return (double?) value + substractValue;
        }
    }
}