using System.ComponentModel;
using System.Windows.Controls;
using Fcog.Core.Forms.Cells;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    public delegate void DeleteCellHandler(Cell cell);

    public interface ICellControl : INotifyPropertyChanged
    {
        Cell Cell { get; set; }
        CellRectangle CellRectangle { get; set; }

        Canvas ImageCanvas { get; set; }

        DeleteCellHandler DeleteCellAction { get; set; }
    }
}