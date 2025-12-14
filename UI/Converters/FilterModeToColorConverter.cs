using CORE;
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
            if (value is not FilterModeEnum mode)
                return Brushes.Black;

            return mode switch
            {
                FilterModeEnum.Equal => Brushes.Green,
                FilterModeEnum.NotEqual => Brushes.Red,

                FilterModeEnum.Greater => Brushes.SteelBlue,
                FilterModeEnum.GreaterOrEqual => Brushes.SteelBlue,

                FilterModeEnum.Less => Brushes.DarkOrange,
                FilterModeEnum.LessOrEqual => Brushes.DarkOrange,

                FilterModeEnum.Contains => Brushes.Green,
                FilterModeEnum.NotContains => Brushes.Red,

                _ => Brushes.Black
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
