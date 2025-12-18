using CommunityToolkit.Mvvm.ComponentModel;
using Strzelecki_Baranowski.DuckApp.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public partial class FilterViewModel : ObservableObject
    {
        [ObservableProperty] string? _propertyName;

        [ObservableProperty] FilterMode _operator;

        [ObservableProperty] FilterType _type;

        [ObservableProperty] string? _value;

        public bool IsMatch(object item)
        {
            if (string.IsNullOrEmpty(Value)) return true;
            if (string.IsNullOrEmpty(PropertyName)) return true;

            var prop = item.GetType().GetProperty(PropertyName);
            var itemValue = prop?.GetValue(item)?.ToString();

            if (itemValue == null) return false;

            if (Type == FilterType.Text)
            {
                switch (Operator)
                {
                    case FilterMode.Contains:
                        return itemValue.ToLower().Contains(Value.ToLower());
                    case FilterMode.NotContains:
                        return !itemValue.ToLower().Contains(Value.ToLower());
                    case FilterMode.Equal:
                        return itemValue.Equals(Value, StringComparison.OrdinalIgnoreCase);
                    case FilterMode.NotEqual:
                        return !itemValue.Equals(Value, StringComparison.OrdinalIgnoreCase);
                    default:
                        return true;
                }
            }
            else
            {
                if (!double.TryParse(itemValue, out var itemNumber))
                    return false;

                if (!double.TryParse(Value, out var filterNumber))

                    switch (Operator)
                    {
                        case FilterMode.Equal:
                            return false;
                        case FilterMode.NotEqual:
                            return true;
                        default:
                            return false;
                    }


                switch (Operator)
                {
                    case FilterMode.Equal:
                        return itemNumber == filterNumber;

                    case FilterMode.NotEqual:
                        return itemNumber != filterNumber;

                    case FilterMode.Greater:
                        return itemNumber > filterNumber;

                    case FilterMode.Less:
                        return itemNumber < filterNumber;

                    case FilterMode.GreaterOrEqual:
                        return itemNumber >= filterNumber;

                    case FilterMode.LessOrEqual:
                        return itemNumber <= filterNumber;

                    default:
                        return true;
                }
            }
        }
    }
}

