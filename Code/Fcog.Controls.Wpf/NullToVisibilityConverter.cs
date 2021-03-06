﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Fcog.Controls.Wpf
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null?Visibility.Visible:Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new  NotImplementedException();
        }
    }
}