using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Fcog.Controls.Wpf.Annotations;

using Fcog.Core.Forms.Cells;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    public partial class UnknownCellControl : ICellControl
    {
        private Cell cell;


        private CellRectangle cellRectangle;

        public UnknownCellControl()
        {
            InitializeComponent();
        }


        public CellRectangle CellRectangle
        {
            get { return cellRectangle; }
            set
            {
                cellRectangle = value;
                OnPropertyChanged();
            }
        }

        public Canvas ImageCanvas { get; set; }


        public DeleteCellHandler DeleteCellAction { get; set; }

        public Cell Cell
        {
            get { return cell; }
            set
            {
                cell = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}