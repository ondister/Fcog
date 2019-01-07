using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Fcog.Core.Recognition
{
    public class DataSetPair
    {
        public byte[] ImageBytes { get; }

        public Character Character{ get; }

        public DataSetPair(byte[] imageBytes, Character character)
        {
            //check data size
            var expectedDataLenght = DataSet.ImageHeight * DataSet.ImageWidth;
            if (imageBytes.Length != expectedDataLenght)
            {
                throw new Exception($"Image data must be {expectedDataLenght} bytes");
            }

            ImageBytes = imageBytes;
            Character = character;
        }


        public  void Save(string folderName)
        {
            const string imageExtension = ".jpg";

            var directoryInfo= new DirectoryInfo(folderName);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(folderName);
            }

           if (Character != null)
            {
                var fileName = Path.ChangeExtension( $"{folderName}{Path.DirectorySeparatorChar}{Character.TextView}_{Character.Index}_{Guid.NewGuid()}",imageExtension);

                SaveBitmap(fileName, DataSet.ImageWidth, DataSet.ImageHeight, ImageBytes);
            }
           

        }

        private void SaveBitmap(string fileName, int width, int height, byte[] imageData)
        {
            const int imageDimention = 4; //RGB & Alfa Channels
         
            var data = new byte[width * height * imageDimention];

           var dataIndex = 0;

            for (var index = 0; index < width * height; index++)
            {
                var value = imageData[index];

                data[dataIndex++] = value;
                data[dataIndex++] = value;
                data[dataIndex++] = value;
                data[dataIndex++] = 0;
            }

            unsafe
            {
                fixed (byte* ptr = data)
                {

                    using (var image = new Bitmap(width, height, width * imageDimention, PixelFormat.Format32bppRgb, new IntPtr(ptr)))
                    {
                       image.Save(fileName);
                    }
                }
            }



        }
    }
}
