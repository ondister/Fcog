using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

using MahApps.Metro.IconPacks;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    [ValueConversion(typeof(CellRectangle), typeof( PackIconMaterial))]
    public  class CellRectangleToImageConverter: IValueConverter
    {
      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.VectorSquareSolid };
            var cellRectangle = value as CellRectangle;
            if (cellRectangle != null)
            {
                var icon= cellRectangle.IsEmpty?new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.VectorSquareSolid } : new PackIconFontAwesome { Kind = PackIconFontAwesomeKind.SquareFullSolid};
                result = icon;
            }
           
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
