using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Fcog.Core.Forms;

namespace Fcog.Controls.Wpf.Forms
{
    [ValueConversion(typeof(WorkMode), typeof(Visibility))]
    public class WorkModeCreateToVisibleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = Visibility.Collapsed;

            if (value != null && (WorkMode) value == WorkMode.CreateForm)
                result = Visibility.Visible;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = WorkMode.Unknown;
            if (value != null && (Visibility) value == Visibility.Visible)
                result = WorkMode.CreateForm;
            return result;
        }
    }
}