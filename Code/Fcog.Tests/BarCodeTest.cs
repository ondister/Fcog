using System.Drawing;
using System.IO;
using Fcog.Core.Barcodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fcog.Tests
{
    [TestClass]
    public class BarCodeTest
    {
        [TestMethod]
        public void CreateBarCodeTest()
        {
            var formId= 1111111111;
            var formIndex = 1;
            var fileName = "barcode.jpeg";

            BarCode.Create(fileName,formId,formIndex);
            var fileInfo = new FileInfo(fileName);

            Assert.IsTrue(fileInfo.Exists);
        
        }

        [TestMethod]
        public void ReadBarCodeTest()
        {
           CreateBarCodeTest();
            var formId = 1111111111;
            var formIndex = 1;
            var expectedContext = $"{formId}-{formIndex}";
            var fileName = "barcode.jpeg";
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                var bitmap = new Bitmap(fileName);
                var context = BarCode.Decode(bitmap);

                Assert.AreEqual(expectedContext,context);
            }
            else
            {
                Assert.Fail("Barcode file not created");
            }
        }
    }
}
