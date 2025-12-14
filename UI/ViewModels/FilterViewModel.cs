using CommunityToolkit.Mvvm.ComponentModel;
using CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    public partial class FilterViewModel : ObservableObject
    {
        [ObservableProperty] string _propertyName;

        [ObservableProperty] FilterModeEnum _operator;

        [ObservableProperty] FilterTypeEnum _type;

        [ObservableProperty] string _value;

        public bool IsMatch(object item)
        {
            if (string.IsNullOrEmpty(Value)) return true;

            var prop = item.GetType().GetProperty(PropertyName);
            var itemValue = prop?.GetValue(item)?.ToString();

            if (itemValue == null) return false;

            if (Type==FilterTypeEnum.Text)
            {
                switch (Operator)
                {
                    case FilterModeEnum.Contains:
                        return itemValue.ToLower().Contains(Value.ToLower());
                    case FilterModeEnum.NotContains:
                        return !itemValue.ToLower().Contains(Value.ToLower());
                    case FilterModeEnum.Equal:
                        return itemValue.Equals(Value, StringComparison.OrdinalIgnoreCase);
                    case FilterModeEnum.NotEqual:
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
                    return false;

                switch (Operator)
                {
                    case FilterModeEnum.Equal:
                        return itemNumber == filterNumber;

                    case FilterModeEnum.NotEqual:
                        return itemNumber != filterNumber;

                    case FilterModeEnum.Greater:
                        return itemNumber > filterNumber;

                    case FilterModeEnum.Less:
                        return itemNumber < filterNumber;

                    case FilterModeEnum.GreaterOrEqual:
                        return itemNumber >= filterNumber;

                    case FilterModeEnum.LessOrEqual:
                        return itemNumber <= filterNumber;

                    default:
                        return true;
                }
            }
        }
    }
}
