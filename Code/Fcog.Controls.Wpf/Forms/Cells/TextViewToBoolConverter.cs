using Fcog.Core.Forms.Cells.Content;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    [ValueConversion(typeof(TextView), typeof(bool?))]
    public class TextViewToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? result = false;
            if (value is TextView textView)
            {
                result = TextView.ToBool(textView);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = TextViews.Empty;
            if (value is bool?)
            {
                var boolValue = value as bool?;
                result = TextView.FromBool(boolValue);
            }

            return result;
        }
    }
}