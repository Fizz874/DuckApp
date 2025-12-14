using Strzelecki_Baranowski.DuckApp.CORE;
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

        public FilterMode FilterMode
        {
            get => (FilterMode)GetValue(FilterModeProperty);
            set => SetValue(FilterModeProperty, value);
        }

        public static readonly DependencyProperty FilterModeProperty = DependencyProperty.Register(
            nameof(FilterMode),
            typeof(FilterMode),
            typeof(FilterControl),
            new PropertyMetadata(FilterMode.Equal));

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

        public FilterType FilterType
        {
            get => (FilterType)GetValue(FilterTypeProperty);
            set => SetValue(FilterTypeProperty, value);
        }

        public static readonly DependencyProperty FilterTypeProperty = DependencyProperty.Register(
            nameof(FilterType),
            typeof(FilterType),
            typeof(FilterControl),
            new PropertyMetadata(FilterType.Text, OnFilterTypeChanged));

        public ObservableCollection<FilterMode> AvailableModes
        {
            get => (ObservableCollection<FilterMode>)GetValue(AvailableModesProperty);
            private set => SetValue(AvailableModesKey, value);
        }

        private static readonly DependencyPropertyKey AvailableModesKey = DependencyProperty.RegisterReadOnly(
            nameof(AvailableModes),
            typeof(ObservableCollection<FilterMode>),
            typeof(FilterControl),
            new PropertyMetadata(new ObservableCollection<FilterMode>()));

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
            var newModes = new ObservableCollection<FilterMode>();

            if (FilterType == FilterType.Text)
            {
                newModes.Add(FilterMode.Contains);
                newModes.Add(FilterMode.NotContains);
                newModes.Add(FilterMode.Equal);
                newModes.Add(FilterMode.NotEqual);
            }
            else if (FilterType == FilterType.Number)
            {
                newModes.Add(FilterMode.Equal);
                newModes.Add(FilterMode.NotEqual);
                newModes.Add(FilterMode.Greater);
                newModes.Add(FilterMode.Less);
                newModes.Add(FilterMode.GreaterOrEqual);
                newModes.Add(FilterMode.LessOrEqual);
            }

            AvailableModes = newModes;

            if (!AvailableModes.Contains(FilterMode))
            {
                FilterMode = AvailableModes[0];
            }
        }
    }
}
