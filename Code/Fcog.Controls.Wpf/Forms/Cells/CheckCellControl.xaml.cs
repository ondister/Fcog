using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using Fcog.Controls.Wpf.Annotations;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Rectangle = System.Drawing.Rectangle;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    
    public partial class CheckCellControl : ICellControl
    {
        private Cell cell;
       
        public CheckCellControl()
        {
            InitializeComponent();
        }

        public Cell Cell
        {
            get { return cell; }
            set
            {
                cell = value;
                if (cell != null)
                {
                    cell.Recognized += Cell_Recognized;
                }
                OnPropertyChanged();

            }
        }

        private void Cell_Recognized(object sender, EventArgs e)
        {
          SetRectangle();
        }

        private void SetRectangle()
        {
            CellRectangle = new CellRectangle(Cell.Rectangle.Height, Cell.Rectangle.Width, Cell.Rectangle.Y, Cell.Rectangle.X);
            Canvas.SetLeft(CellRectangle.RectangleShape, CellRectangle.X);
            Canvas.SetTop(CellRectangle.RectangleShape, CellRectangle.Y);

            var binding = new Binding(nameof(Cell.Content.TextView))
            {
                Source = Cell.Content,
                Converter = new TextViewToBrushConverter(),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            BindingOperations.SetBinding(CellRectangle.RectangleShape, Shape.FillProperty, binding);

            //add menu
            if (FindResource("ContentMenu") is ContextMenu contextMenu)
            {
                CellRectangle.RectangleShape.ContextMenu = contextMenu;
            }

            ImageCanvas.Children.Add(CellRectangle.RectangleShape);


        }


        public CellRectangle CellRectangle
        {
            get { return cellRectangle; }
            set { cellRectangle = value; OnPropertyChanged();}
        }

        public Canvas ImageCanvas { get; set; }

        public DeleteCellHandler DeleteCellAction { get; set; }


        private CellRectangle cellRectangle;
      

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void MenuItemMiss_OnClick(object sender, RoutedEventArgs e)
        {
            Cell.Content.TextView = TextViews.Miss;
        }

        private void MenuItemEmpty_OnClick(object sender, RoutedEventArgs e)
        {
            Cell.Content.TextView = TextViews.Empty;
        }

        private void MenuItemMark_OnClick(object sender, RoutedEventArgs e)
        {
            Cell.Content.TextView = TextViews.Mark;
        }

        #region Draw Rectangle & Find Blob

        private void ButtonFindBlob_OnClick(object sender, RoutedEventArgs e)
        {
            if (ImageCanvas != null)
            {
                var painter = new CellPainter(ImageCanvas);
                painter.CellDrawed += Painter_CellDrawed;
                painter.StartDraw(CellRectangle);
            }
        }

        private void Painter_CellDrawed(CellRectangle rectangle)
        {
            CellRectangle = rectangle;
            Cell.Rectangle = new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width,
                (int)rectangle.Height);
            Cell.FindBlobInRectangle();

            CellRectangle.X = Cell.Rectangle.X;
            CellRectangle.Y = Cell.Rectangle.Y;
            CellRectangle.Width = Cell.Rectangle.Width;
            CellRectangle.Height = Cell.Rectangle.Height;

            OnPropertyChanged(nameof(CellRectangle));
        }

        #endregion

        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteCellAction(Cell);
        }

      

        private void ButtonSendData_OnClick(object sender, RoutedEventArgs e)
        {
           Cell.AddContentToDataset();
        }

        private void ButtonSelectCell_OnClick(object sender, RoutedEventArgs e)
        {
            CellRectangle?.StartSelectAnimation();
        }
    }
}