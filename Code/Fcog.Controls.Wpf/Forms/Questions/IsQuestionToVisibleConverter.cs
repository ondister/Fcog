using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Questions;

namespace Fcog.Controls.Wpf.Forms.Questions
{
    [ValueConversion(typeof(Type), typeof(Visibility))]
    public class IsQuestionToVisibleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = Visibility.Collapsed;

            if (value != null)
            {
                   result = Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           throw new NotImplementedException();
        }
    }
}
