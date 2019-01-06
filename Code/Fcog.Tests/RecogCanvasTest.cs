using System;
using System.Collections.Generic;
using System.Drawing;
using Fcog.Core.Recognition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fcog.Tests
{
    [TestClass]
    public class RecogCanvasTest
    {
        [TestMethod]
        public void RecognizeFormTest()
        {
            var testImagesList = new List<Bitmap>();

            for (var i =1; i <= 4; i++)
            {
                var testImage=new Bitmap($@"TestImages\scan{i}.jpeg");
                testImagesList.Add(testImage);
            }
            var nameIndex = 1;
            
                foreach (var image in testImagesList)
            {
                try
                {
                    //var recogCanvas = new RecogCanvas(image, 7d, 80d, 5d, 15d);
                    //recogCanvas.FindMarker();
                    //Console.WriteLine($"{nameIndex}.jpeg have {recogCanvas.Marker.BarCodeContext} code");
                   
                }
                catch (FormRecognizeException ex)
                {
                   Console.WriteLine($"{nameIndex}.jpeg have {ex} error");
                }
                finally
                {
                    nameIndex++;
                }

            }

            
        }
    }
}
