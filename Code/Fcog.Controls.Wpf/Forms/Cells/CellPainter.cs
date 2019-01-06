using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Fcog.Controls.Wpf.Forms.Cells
{
    internal class CellPainter
    {
        private readonly Canvas canvasImage;
        private CellRectangle cellRectangle;
        private bool drawMode;
        private Point mouseStartPosition;


        internal CellPainter(Canvas canvasImage)
        {
            this.canvasImage = canvasImage;
            
        }

        internal event DrawCellHandler CellDrawed;

        internal void StartDraw(CellRectangle rectangle)
        {
            if (rectangle != null)
            {
                canvasImage.Children.Remove(rectangle.RectangleShape);
            }

            drawMode = true;
            Mouse.OverrideCursor = Cursors.Cross;
            EventsSubscribe();
        }

    

        private void EventsSubscribe()
        {
            canvasImage.MouseLeftButtonDown += CanvasImage_MouseLeftButtonDown;
            canvasImage.MouseMove += CanvasImage_MouseMove;
            canvasImage.MouseLeftButtonUp += CanvasImage_MouseLeftButtonUp;
        }

        private void EventsUnSubscribe()
        {
            canvasImage.MouseLeftButtonDown -= CanvasImage_MouseLeftButtonDown;
            canvasImage.MouseMove -= CanvasImage_MouseMove;
            canvasImage.MouseLeftButtonUp -= CanvasImage_MouseLeftButtonUp;
        }


        private void CanvasImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (drawMode)
            {
                Mouse.OverrideCursor = null;
                drawMode = false;
                EventsUnSubscribe();
                OnCellDrawed(cellRectangle);
            }
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawMode && cellRectangle != null)
            {
                var mouseCurrentPosition = e.GetPosition(canvasImage);
                var dX = mouseCurrentPosition.X - mouseStartPosition.X;
                var dY = mouseCurrentPosition.Y - mouseStartPosition.Y;
                if (dX >= 0 && dY >= 0)
                {
                    cellRectangle.Height = dY;
                    cellRectangle.Width = dX;
                }
            }
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawMode)
            {
                mouseStartPosition = e.GetPosition(canvasImage);

                    cellRectangle = new CellRectangle(0, 0, mouseStartPosition.Y, mouseStartPosition.X);
                    canvasImage.Children.Add(cellRectangle.RectangleShape);
             

            }
        }


        protected virtual void OnCellDrawed(CellRectangle rectangle)
        {
            CellDrawed?.Invoke(rectangle);
        }

        internal delegate void DrawCellHandler(CellRectangle rectangle);
    }
}