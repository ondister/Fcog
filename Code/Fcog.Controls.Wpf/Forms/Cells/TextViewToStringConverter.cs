using Fcog.Core.Forms.Cells.Content;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    [ValueConversion(typeof(TextView), typeof(string))]
    public class TextViewToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            if (value is TextView textView)
            {
                result = textView.Text;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new TextView(string.Empty);
            if (value is string stringValue)
            {
                result = new TextView(stringValue);
            }

            return result;
        }
    }
}