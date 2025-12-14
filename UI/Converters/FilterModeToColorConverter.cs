using Strzelecki_Baranowski.DuckApp.CORE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class FilterModeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not FilterMode mode)
                return Brushes.Black;

            return mode switch
            {
                FilterMode.Equal => Brushes.Green,
                FilterMode.NotEqual => Brushes.Red,

                FilterMode.Greater => Brushes.SteelBlue,
                FilterMode.GreaterOrEqual => Brushes.SteelBlue,

                FilterMode.Less => Brushes.DarkOrange,
                FilterMode.LessOrEqual => Brushes.DarkOrange,

                FilterMode.Contains => Brushes.Green,
                FilterMode.NotContains => Brushes.Red,

                _ => Brushes.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
