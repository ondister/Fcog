using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fcog.Core.Recognition
{
    public class DataSetPair
    {
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

        public byte[] ImageBytes { get; }
        public Bitmap Bitmap => ToBitmap(DataSet.ImageWidth, DataSet.ImageHeight, ImageBytes);

        public int ImageHeight => DataSet.ImageHeight;

        public int ImageWidth => DataSet.ImageWidth;

        public Character Character { get; }


        public void Save(string folderName)
        {
            const string imageExtension = ".jpg";

            var directoryInfo = new DirectoryInfo(folderName);
            if (!directoryInfo.Exists)
            {
                Directory.CreateDirectory(folderName);
            }

            if (Character != null)
            {
                var fileName =
                    Path.ChangeExtension(
                        $"{folderName}{Path.DirectorySeparatorChar}{Character.TextView}_{Character.Index}_{Guid.NewGuid()}",
                        imageExtension);

                SaveBitmap(fileName, DataSet.ImageWidth, DataSet.ImageHeight, ImageBytes);
            }
        }

        private Bitmap ToBitmap(int width, int height, byte[] imageData)
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
                    var image = new Bitmap(width, height, width * imageDimention, PixelFormat.Format32bppRgb,
                        new IntPtr(ptr));
                    return image;
                }
            }
        }


        private void SaveBitmap(string fileName, int width, int height, byte[] imageData)
        {
            using (var image = ToBitmap(width, height, imageData))
            {
                image.Save(fileName);
            }
        }
    }
}