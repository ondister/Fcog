using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Accord;
using Accord.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.Rendering;

namespace Fcog.Core.Barcodes
{
  public static class BarCode
  {
      private const int imageHight = 25;
      private const int imageWidth = 300;
      private const int borderthickness = 2;

      private const int fontSize = 6;

      private const int dpi = 100;

      public static void Create(string fileName, int questionnaireId, int formIndex)
        {
            var barCodeWriter = new BarcodeWriter<Bitmap>
            {
                Encoder = new Code128Writer(),
                Format = BarcodeFormat.CODE_128,
                Renderer = new BitmapRenderer
                {
                    DpiX = dpi,
                    DpiY = dpi,
                    TextFont = new Font(FontFamily.GenericSansSerif, fontSize)
                },
                Options = new EncodingOptions {Height = imageHight, Width = imageWidth}
            };
            var bitmap = barCodeWriter.Write($"{questionnaireId}-{formIndex}");
            
            bitmap = DrawBorders(bitmap);
            
            bitmap.Save(fileName);
        }


      private static  Bitmap DrawBorders(Bitmap image)
      {
          var imageForBorders = image;
          var bitmapdata = imageForBorders.LockBits(ImageLockMode.ReadWrite);

          for (var i = 0; i <= borderthickness; i++)
          {
              //left
              Drawing.Line(bitmapdata, new IntPoint(i,0), new IntPoint(i, imageHight),Color.Black);
                //up
              Drawing.Line(bitmapdata, new IntPoint(0, i), new IntPoint(imageWidth, i), Color.Black);
                //right
              Drawing.Line(bitmapdata, new IntPoint(imageWidth - i, 0), new IntPoint(imageWidth - i, imageHight), Color.Black);
                //bottom
              Drawing.Line(bitmapdata, new IntPoint(0, imageHight - i), new IntPoint(imageWidth, imageHight - i), Color.Black);
            }
          
            imageForBorders.UnlockBits(bitmapdata);

          return imageForBorders;
      }

      public static string Decode(Bitmap image)
        {
           var result = string.Empty;

           var reader = new BarcodeReader();
           var readResult = reader.Decode(image);
           if (readResult != null)
            {
               result= readResult.Text;
            }

            return result;
        }
    }
}
