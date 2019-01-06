using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Fcog.Controls.Wpf.Annotations;
using RectangleShape = System.Windows.Shapes.Rectangle;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    public  class CellRectangle : INotifyPropertyChanged
    {
        private readonly Brush fillBrush = Brushes.Blue;
        private const double opacity = 0.6;

        private double height;
        private double x;
        private double y;
        private double width;

        public bool IsEmpty =>   Height == 0 && Width == 0;

        public CellRectangle(double height, double width, double top, double left)
        {
            Height = height;
            Width = width;
            Y = top;
            X = left;

            RectangleShape = new RectangleShape
            {
                Fill = fillBrush,
                Opacity = opacity,
                Height = Height,
                Width = Width
            };

            Canvas.SetTop(RectangleShape, Y);
            Canvas.SetLeft(RectangleShape, X);
        }

        public RectangleShape RectangleShape { get; }

        public void StartSelectAnimation()
        {

            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                From = opacity,
                By = -0.6,
                FillBehavior = FillBehavior.Stop,
                AccelerationRatio = 0.9,
                RepeatBehavior = new RepeatBehavior(5),
                AutoReverse = true
            };
            RectangleShape.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                if (RectangleShape != null)
                {
                    RectangleShape.Height = height;
                }
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                if (RectangleShape != null)
                {
                    RectangleShape.Width = width;
                }
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                if (RectangleShape != null)
                {
                    Canvas.SetTop(RectangleShape, Y);
                }
                OnPropertyChanged();
            }
        }

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                if (RectangleShape != null)
                {
                    Canvas.SetLeft(RectangleShape, X);
                }
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