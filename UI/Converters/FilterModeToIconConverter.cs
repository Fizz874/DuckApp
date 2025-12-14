using CORE;
using Strzelecki_Baranowski.DuckApp.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class FilterModeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterModeEnum mode)
            {
                return mode switch
                {
                    FilterModeEnum.Equal => "=",
                    FilterModeEnum.NotEqual => "≠",
                    FilterModeEnum.Greater => ">",
                    FilterModeEnum.Less => "<",
                    FilterModeEnum.GreaterOrEqual => "≥",
                    FilterModeEnum.LessOrEqual => "≤",
                    FilterModeEnum.Contains => "⊆", // Ikona "zawiera tekst"
                    FilterModeEnum.NotContains => "⊈",
                    _ => "?"
                };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
