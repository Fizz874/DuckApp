using CORE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Strzelecki_Baranowski.DuckApp.UI
{

    public class FilterControl : Control
    {
        static FilterControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterControl), new FrameworkPropertyMetadata(typeof(FilterControl)));
        }
        //public enum FilterModeEnum
        //{
        //    Equal,
        //    Greater,
        //    Less,
        //    NotEqual,
        //    GreaterOrEqual,
        //    LessOrEqual,
        //    Contains,
        //    NotContains
        //}

        //public enum FilterTypeEnum
        //{
        //    Text,
        //    Number
        //}

        public FilterModeEnum FilterMode
        {
            get => (FilterModeEnum)GetValue(FilterModeProperty);
            set => SetValue(FilterModeProperty, value);
        }

        public static readonly DependencyProperty FilterModeProperty = DependencyProperty.Register(
            nameof(FilterMode),
            typeof(FilterModeEnum),
            typeof(FilterControl),
            new PropertyMetadata(FilterModeEnum.Equal));

        public string FilterValue
        {
            get => (string)GetValue(FilterValueProperty);
            set => SetValue(FilterValueProperty, value);
        }

        public static readonly DependencyProperty FilterValueProperty = DependencyProperty.Register(
            nameof(FilterValue),
            typeof(string),
            typeof(FilterControl),
            new PropertyMetadata(null));

        public FilterTypeEnum FilterType
        {
            get => (FilterTypeEnum)GetValue(FilterTypeProperty);
            set => SetValue(FilterTypeProperty, value);
        }

        public static readonly DependencyProperty FilterTypeProperty = DependencyProperty.Register(
            nameof(FilterType),
            typeof(FilterTypeEnum),
            typeof(FilterControl),
            new PropertyMetadata(FilterTypeEnum.Text, OnFilterTypeChanged));

        public ObservableCollection<FilterModeEnum> AvailableModes
        {
            get => (ObservableCollection<FilterModeEnum>)GetValue(AvailableModesProperty);
            private set => SetValue(AvailableModesKey, value);
        }

        private static readonly DependencyPropertyKey AvailableModesKey = DependencyProperty.RegisterReadOnly(
            nameof(AvailableModes),
            typeof(ObservableCollection<FilterModeEnum>),
            typeof(FilterControl),
            new PropertyMetadata(new ObservableCollection<FilterModeEnum>()));

        public static readonly DependencyProperty AvailableModesProperty = AvailableModesKey.DependencyProperty;




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateAvailableModes();
        }

        private static void OnFilterTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FilterControl control)
            {
                control.UpdateAvailableModes();
            }
        }

        private void UpdateAvailableModes()
        {
            var newModes = new ObservableCollection<FilterModeEnum>();

            if (FilterType == FilterTypeEnum.Text)
            {
                newModes.Add(FilterModeEnum.Contains);
                newModes.Add(FilterModeEnum.NotContains);
                newModes.Add(FilterModeEnum.Equal);
                newModes.Add(FilterModeEnum.NotEqual);
            }
            else if (FilterType == FilterTypeEnum.Number)
            {
                newModes.Add(FilterModeEnum.Equal);
                newModes.Add(FilterModeEnum.NotEqual);
                newModes.Add(FilterModeEnum.Greater);
                newModes.Add(FilterModeEnum.Less);
                newModes.Add(FilterModeEnum.GreaterOrEqual);
                newModes.Add(FilterModeEnum.LessOrEqual);
            }

            AvailableModes = newModes;

            if (!AvailableModes.Contains(FilterMode))
            {
                FilterMode = AvailableModes[0];
            }
        }
    }
}
