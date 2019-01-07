﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Accord.Imaging;
using Fcog.Core.Annotations;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Cells;
using Point = Accord.Point;

namespace Fcog.Core.Forms.Cells
{
   
    public abstract class Cell : IRecognizable,INotifyPropertyChanged, IEquatable<Cell>, IWrapped<CellWrapper>
    {

        public RecogMachine RecogMachine => recogMachine;
        protected RecogMachine recogMachine;


        internal void SetRecogMachine(RecogMachine recogMachine)
        {
            this.recogMachine = recogMachine;
        }

        public event EventHandler Recognized;

        private CellContent content;
        private MarkerDistance distanceFromMarker;
        private int index;

        private string label;
        private Rectangle rectangle;

      
        protected Cell(RecogTools recogTools, RecogMachine recogMachine)
        {
            Guid = Guid.NewGuid();
            RecogTools = recogTools;
            this.recogMachine = recogMachine;
        }

        protected Cell(Guid guid, RecogTools recogTools, RecogMachine recogMachine)
        {
            Guid = guid;
            RecogTools = recogTools;
            this.recogMachine = recogMachine;
        }


        public Guid Guid { get; }

      
        public string Label
        {
            get => label;
            set
            {
                label = value;
                OnPropertyChanged();
            }
        }

 
        public CellContent Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }


        public int Index
        {
            get => index;
            set
            {
                index = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Distance from marker center of gravity to cell center of gravity
        /// </summary>

        public MarkerDistance DistanceFromMarker
        {
            get => distanceFromMarker;
            set
            {
                distanceFromMarker = value;
                OnPropertyChanged();
            }
        }


        public Rectangle Rectangle
        {
            get => rectangle;
            set
            {
                rectangle = value;
                OnPropertyChanged();
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private const int minRectangleSize = 3;

        public void FindBlobInRectangle()
        {
            if (Rectangle != Rectangle.Empty && Rectangle.Height>= minRectangleSize && Rectangle.Width>= minRectangleSize)
            {
                var invertedImage = RecogTools.InvertedImage;
                //lock image in memory
                var bitmapData = invertedImage.LockBits(Rectangle, ImageLockMode.ReadWrite, invertedImage.PixelFormat);

                //set filter for markers search 
                var blobCounter = new BlobCounter
                {
                    FilterBlobs = false
                };

                //search markers
                blobCounter.ProcessImage(bitmapData);
                var blobs = blobCounter.GetObjectsInformation();
                invertedImage.UnlockBits(bitmapData);

                if (blobs.Any())
                {
                    var maxBlobArea = blobs.Max(b => b.Area);
                    var blob = blobs.FirstOrDefault(b => b.Area == maxBlobArea);
                    if (blob != null)
                    {
                        var x = Rectangle.X + blob.Rectangle.X;
                        var y = Rectangle.Y + blob.Rectangle.Y;
                        Rectangle = new Rectangle(x, y, blob.Rectangle.Width, blob.Rectangle.Height);
                        DistanceFromMarker = FindDistanceFromMarker();
                    }
                }
            }
        }

        private MarkerDistance FindDistanceFromMarker()
        {
          var markerCenterOfGravirty=  RecogTools.Marker.Blob.CenterOfGravity;
            var xMarker = markerCenterOfGravirty.X;
            var yMarker = markerCenterOfGravirty.Y;

            //Y of the cell rectangle is always меньше
            var deltaY = Rectangle.Y- yMarker ;
            var deltaX = Rectangle.X - xMarker ;
           
            return new MarkerDistance((int)deltaX, (int)deltaY);
        }

        private Rectangle MarkerDistanceToRectangle(Rectangle cellRectangle, MarkerDistance markerDistance, Point markerCenterOfGravity)
        {
            var result = cellRectangle;
            if (markerDistance != null)
            {
                result.X = (int)markerCenterOfGravity.X +markerDistance.Left;
                result.Y = (int)markerCenterOfGravity.Y+markerDistance.Top;
            }

            return result;
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IEquatable implement

        public abstract CellWrapper Wrap();
     

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Cell) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Guid.Equals(other.Guid);
        }

     
        #endregion


        public RecogTools RecogTools { get; set; }

        public virtual void Recognize()
        {

            //restore rectangle position
            Rectangle = MarkerDistanceToRectangle(Rectangle, DistanceFromMarker, RecogTools.Marker.CenterOfGravity);
            Rectangle = CorrectRectanglePosition(Rectangle);
            
            // crop image for content
            var cellBitmap = RecogTools.InvertedImage.Clone(Rectangle, RecogTools.ImageForRecognize.PixelFormat);
            //and recognize
            Content = RecogMachine.Recognize(cellBitmap);

            //call event
            OnRecognized();
        }

        public Rectangle CorrectRectanglePosition(Rectangle cellRectangle)
        {
#warning Переделать коррекцию позиции квадрата, с учетом деформации листа: сначала берем больший квадрат, затем ищем в нем реальную ячейку
            return cellRectangle;
        }

        protected virtual void OnRecognized()
        {
            Recognized?.Invoke(this, EventArgs.Empty);
        }
    }
}