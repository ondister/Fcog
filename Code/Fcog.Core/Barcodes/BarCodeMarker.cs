using System;
using System.Drawing;
using System.Runtime.Serialization;
using Accord;
using Accord.Imaging;
using Fcog.Core.Serialization;

namespace Fcog.Core.Barcodes
{

    public class BarCodeMarker:IWrapped<BarCodeMarkerWrapper>
    {
        //private const int barcodeBordersThickness=5;

        public BarCodeMarker(Blob blob, Bitmap image)
        {
            Blob = blob;
            CenterOfGravity = Blob.CenterOfGravity;
            //   var rectangle=new Rectangle(blob.Rectangle.X + barcodeBordersThickness, blob.Rectangle.Y + barcodeBordersThickness, blob.Rectangle.Width - barcodeBordersThickness*2, blob.Rectangle.Height - barcodeBordersThickness*2);
            var rectangle = new Rectangle(blob.Rectangle.X, blob.Rectangle.Y, blob.Rectangle.Width, blob.Rectangle.Height);
            var barCodeImage = image.Clone(rectangle, image.PixelFormat);
            BarCodeImage = barCodeImage;
            BarCodeContext = BarCode.Decode(barCodeImage);
        }

        internal BarCodeMarker( string context, float centerOfGravityX, float centerOfGravityY )
        {
            CenterOfGravity= new Accord.Point(centerOfGravityX,centerOfGravityY);
            BarCodeContext = context;
        }

        public Accord.Point CenterOfGravity { get;  }

        public Bitmap BarCodeImage { get;  }


        public string BarCodeContext { get; }


        public Blob Blob { get; }

        public BarCodeMarkerWrapper Wrap()
        {
            var adapter = new BarCodeMarkerWrapper
            {
                BarCodeContext = BarCodeContext,
                CenterOfGravityX = Blob.CenterOfGravity.X,
                CenterOfGravityY = Blob.CenterOfGravity.Y
            };
            return adapter;
        }

       
    }
}