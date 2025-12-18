using Strzelecki_Baranowski.DuckApp.CORE;
using System.Globalization;
using System.Windows.Data;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public class FilterModeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterMode mode)
            {
                return mode switch
                {
                    FilterMode.Equal => "=",
                    FilterMode.NotEqual => "≠",
                    FilterMode.Greater => ">",
                    FilterMode.Less => "<",
                    FilterMode.GreaterOrEqual => "≥",
                    FilterMode.LessOrEqual => "≤",
                    FilterMode.Contains => "⊆",
                    FilterMode.NotContains => "⊈",
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
