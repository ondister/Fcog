using System;
using System.Drawing;
using System.Linq;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Forms.Questions;
using Fcog.Core.IO.Templates.Readers;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Recognition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fcog.Tests
{
    [TestClass]
    public class WrappersTest
    {

        private const string machinesPath = @"../../RecogMachines";
        private const string testImageFileName = @"../../FormsExamples/TestFormFill2.jpeg";
        private const int formId = 999999999;   //as barcode of test image form id and index
        private const int formIndex = 1;

        private const string markMachineGuid= "b16873e7-95c1-4af9-9b12-0ef85b128eb4";
        private const string mnistMachineGuid = "09c2f6b6-9db7-4afe-a1d4-64ce1156e3ba";

        [TestMethod]
        public void QWrapperSmokeTest()
        {
            #region Store

            var store = new RecogMachinesFileStore(machinesPath);
            RecogMachinesPool.Instance.InitializeStoreAsync(store).Wait();
            var markMachine = store.GetRecogMachineAsync(Guid.Parse(markMachineGuid)).Result;
            var mnistMachine = store.GetRecogMachineAsync(Guid.Parse(mnistMachineGuid)).Result;
            Assert.IsNotNull(markMachine);
            Assert.IsNotNull(mnistMachine);

            #endregion


            #region Set properties
            //set basic properties
            var qProperties = new QuestionnareProperties
            {
                Guid = Guid.NewGuid(),
                Author = "Kaeshko Alexey",
                CreationDateTime = new DateTime(2018, 11, 11),
                Description = "Test questionnaire description",
                Name = "Test questionnaire name",
                Version = new QVersion(0, 0, 1)
            };

            //set recogForm properties
            var rProperties = new RecogFormProperties
            {
                FormId = 0,
                FormIndex = 0,
                MarkerHeight = 8d,
                MarkerHeightTolerance = 3d,
                MarkerWidth = 80d,
                MarkerWidthTolerance = 10d
            };

            #endregion


            #region Questionnaire creation

            //create new questionnaire in create form mode
            var questionnaire = new QModel(qProperties);
            var recogForm = questionnaire.AddRecogForm(rProperties);//add the recogForm
            #endregion

            //add image for recognition
            recogForm.AddImage(new Bitmap(testImageFileName));

            //find marker
            recogForm.FindMarker();


            #region Check marker recognition

            Assert.IsNotNull(recogForm.RecogTools.Marker);
            Assert.AreEqual(formId, recogForm.Properties.FormId);
            Assert.AreEqual(formIndex, recogForm.Properties.FormIndex);

            #endregion

            #region Add questions

            //add markQuestion
            //mark question always have single cell
            var markQuestion = recogForm.AddQuestion(typeof(MarkQuestion), "Mark question label", markMachine);
            markQuestion.Cells.Single().DistanceFromMarker = new MarkerDistance(0, 0);
            //markQuestion.AddCell("Mark question cell").DistanceFromMarker = new MarkerDistance(0, 0);


            //add multiQuestion
            var multiQuestion = recogForm.AddQuestion(typeof(MultiQuestion), "Multi question label",  markMachine);
            multiQuestion.AddCell("Multi question cell 1").DistanceFromMarker = new MarkerDistance(0, 0);
            multiQuestion.AddCell("Multi question cell 2").DistanceFromMarker = new MarkerDistance(0, 0);


            //add RecogTextQuestion
            var recogTextQuestion = recogForm.AddQuestion(typeof(RecogTextQuestion), "RecogText question label",mnistMachine);
            recogTextQuestion.AddCell("RecogText question cell 1").DistanceFromMarker = new MarkerDistance(0, 0);
            recogTextQuestion.AddCell("RecogText question cell 2").DistanceFromMarker = new MarkerDistance(0, 0);

            //add SingleQuestion
            var singleQuestion = recogForm.AddQuestion(typeof(SingleQuestion), "Single question label", markMachine);
            singleQuestion.AddCell("Single question cell 1").DistanceFromMarker = new MarkerDistance(0, 0);
            singleQuestion.AddCell("Single question cell 2").DistanceFromMarker = new MarkerDistance(0, 0);

            //add TextQuestion
            var textQuestion = recogForm.AddQuestion(typeof(TextQuestion), "Text question label", markMachine);
            textQuestion.AddCell("Text question cell 1").DistanceFromMarker = new MarkerDistance(0, 0);

            #endregion



            //wrap questionnaire
            var wrappedQuestionnaire = questionnaire.Wrap();

            //unwrap questionnaire
            var unwrappedQuestionnaire = wrappedQuestionnaire.UnWrap();
           
            var unwrappedrecogForm = unwrappedQuestionnaire.RecogForms.Single();

            unwrappedrecogForm.AddImage(new Bitmap(testImageFileName));
            unwrappedrecogForm.FindMarker();

            #region Asserts
#warning make cells & question count check assert
            Assert.AreEqual(questionnaire.RecogForms.Count,unwrappedQuestionnaire.RecogForms.Count);
            Assert.AreEqual(recogForm.Questions.Count, unwrappedrecogForm.Questions.Count);

            foreach (var question in unwrappedrecogForm.Questions)
            {
                Assert.AreSame(unwrappedrecogForm.RecogTools,question.RecogTools);
                Assert.IsNotNull(question.RecogMachine);

                foreach (var cell in question.Cells)
                {
                    Assert.AreSame( question.RecogTools,cell.RecogTools);
                    Assert.IsNotNull(cell.RecogMachine);
                }
            }

            #endregion

            
            var storedir = @"templates";

            var fileWriter = new TemplateFileWriter(storedir);
            var fileReader= new TemplateFileReader(storedir);

            questionnaire.SaveTemplate(fileWriter);
            var readedQuestionnaire =QRecog.OpenFromTemplate(fileReader, questionnaire.Properties.Guid);

            var allreadResult = fileReader.ReadAllProperties();

            Assert.IsNotNull(readedQuestionnaire);
            Assert.IsTrue(allreadResult.Result.Any());


        }
    }
}
