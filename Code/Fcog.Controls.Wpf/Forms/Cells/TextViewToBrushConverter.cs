using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Fcog.Core.Forms.Cells.Content;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    [ValueConversion(typeof(TextView), typeof(Brush))]
    public class TextViewToBrushConverter : IValueConverter
    {
        private static readonly Dictionary<TextView, Brush> brushesDictionary = new Dictionary<TextView, Brush>
        {
            {TextViews.Mark, Brushes.Green},
            {TextViews.Empty, Brushes.Yellow},
            {TextViews.Miss, Brushes.DarkRed}
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush fillBrush = null;
            var textView = value as TextView;
            if (textView != null)
            {
                brushesDictionary.TryGetValue(textView, out fillBrush);
            }
            return fillBrush ?? Brushes.Blue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}