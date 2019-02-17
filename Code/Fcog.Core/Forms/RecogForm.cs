using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Fcog.Core.Annotations;
using Fcog.Core.Barcodes;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;
using Fcog.Core.Units;

namespace Fcog.Core.Forms
{
    /// <summary>
    ///     Recognizable form class.
    /// </summary>


    public class RecogForm : IRecognizable,INotifyPropertyChanged,IWrapped<RecogFormWrapper>
    {
        public event EventHandler<QuestionEventArgs> QuestionAdded;
        public event EventHandler<QuestionEventArgs> QuestionRemoved;

        private const int imageResolution = 100;

        private const double maxSkewRemovalAngle = 45;

        private readonly ObservableCollection<Question> questions;

     
        private RecogTools recogTools;

        public RecogFormProperties Properties { get;}

  
        public ReadOnlyObservableCollection<Question> Questions { get;}


        public RecogTools RecogTools
        {
            get
            {
                return recogTools;
            }
            set
            {
                recogTools = value;

                recogTools.PropertyChanged += delegate
                {
                    UpdateRecogTools();
                    OnPropertyChanged();
                };

                UpdateRecogTools();
                OnPropertyChanged();
            }
        }

        public void Recognize()
        {
          
            foreach (var question in Questions)
            {
               question.Recognize();
            }
           

        }

        public async Task RecognizeAsync()
        {
            foreach (var question in Questions)
            {
               await question.RecognizeAsync();
            }
        }

        private void UpdateRecogTools()
        {
            foreach (var question in questions)
            {
                question.RecogTools = RecogTools;
            }
        }

        public event EventHandler ImageChanged;

        public event EventHandler MarkerFinded;


        protected virtual void OnMarkerFinded()
        {
            MarkerFinded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnImageChanged()
        {
            ImageChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Constructors

        private RecogForm(WorkMode workMode)
        {
            questions = new ObservableCollection<Question>();
            Questions = new ReadOnlyObservableCollection<Question>(questions);
            RecogTools = new RecogTools {WorkMode = workMode};


        }

        internal RecogForm(RecogFormProperties properties, WorkMode workMode) : this(workMode)
        {
            Properties = properties ?? new RecogFormProperties();
        }

        internal RecogForm(double markerHeight, double markerWidth, double markerHeightTolerance,
            double markerWidthTolerance, int formId, int formIndex, WorkMode workMode) : this(workMode)
        {
            Properties = new RecogFormProperties
            {
                FormId = formId,
                FormIndex = formIndex,
                MarkerHeight = markerHeight,
                MarkerWidth = markerWidth,
                MarkerHeightTolerance = markerHeightTolerance,
                MarkerWidthTolerance = markerWidthTolerance
            };
        }

        #endregion


        #region ImageProcessing

      
        public void AddImage(Bitmap image)
        {
            if (image.HorizontalResolution != imageResolution)
            {
                throw new FormRecognizeException($"Image resolution must be {imageResolution} dpi");
              
            }
         
                RecogTools.ImageForRecognize = image.Clone(PixelFormat.Format24bppRgb);
                
            RecogTools.ImageForRecognize.SetResolution(imageResolution, imageResolution);
            OnImageChanged();
        }

        public void FindMarker()
        {
            if (RecogTools.ImageForRecognize == null)
                throw new FormRecognizeException("Load an image");

            try
            {
                ToGrayScale();
                CorrectColorLevels();
                RemoveSkew();
                var marker = FindBarCodeMarker();


                if (!string.IsNullOrEmpty(marker.BarCodeContext))
                {
                    SetFormId(marker.BarCodeContext);
                    RecogTools.Marker = marker;
                    OnPropertyChanged(nameof(RecogTools));
                    OnMarkerFinded();
               }
            }
            catch (FormRecognizeException ex)
            {
                throw new FormRecognizeException("Problem with the recognition of the form marker", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("A general error occurred", ex);
            }
        }


        public async Task FindMarkerAsync()
        {
           await Task.Run(() =>
           {
               FindMarker();
           });
            
        }

        public bool FindAndCheckMarker()
        {
            var result = false;
            if (RecogTools.ImageForRecognize == null)
                throw new FormRecognizeException("Load an image");

            try
            {
                ToGrayScale();
                CorrectColorLevels();
                RemoveSkew();
                var marker = FindBarCodeMarker();


                if (!string.IsNullOrEmpty(marker.BarCodeContext))
                {
                    result= CheckFormId(marker.BarCodeContext);
                    if (result)
                    {
                        RecogTools.Marker = marker;
                        OnMarkerFinded();
                    }
                    else
                    {
                        RecogTools.ImageForRecognize = null;
                        RecogTools.InvertedImage = null;
                    }
                }
            }
            catch (FormRecognizeException ex)
            {
                throw new FormRecognizeException("Problem with the recognition of the form marker", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("A general error occurred", ex);
            }

            return result;
        }

        public async Task<bool> FindAndCheckMarkerAsync()
        {
            var result = false;
            await Task.Run(() =>
            {
                result = FindAndCheckMarker();
            });
            return result;
        }

        private bool CheckFormId(string barCodeContext)
        {
         
            int formId;
            int formIndex;

            var splittedString = barCodeContext.Split('-');

            int.TryParse(splittedString[0], out formId);
            int.TryParse(splittedString[1], out formIndex);

            var  result = (Properties.FormId == formId) && (Properties.FormIndex == formIndex);


            return result;
        }

        private void SetFormId(string barCodeContext)
        {
            int formId;
            int formIndex;

            var splittedString = barCodeContext.Split('-');

            int.TryParse(splittedString[0], out formId);
            int.TryParse(splittedString[1], out formIndex);

            Properties.FormId = formId;
            Properties.FormIndex = formIndex;
        }

        public void RotateLeft()
        {
            RotateImage(90);
        }

        public void RotateRight()
        {
            RotateImage(-90);
        }

        private void RotateImage(double angle)
        {
            if (RecogTools.ImageForRecognize != null && RecogTools.Marker == null) //you can rotate the image only before recognition
            {
                var rotateFilter = new RotateBilinear(angle);
                RecogTools.ImageForRecognize = rotateFilter.Apply(RecogTools.ImageForRecognize);
                OnImageChanged();
            }
        }

        private Bitmap ResizeImage(Bitmap image)
        {
            var verticalResolution = image.VerticalResolution;
            var koeffitient = verticalResolution / 100;
            var width = image.Width / koeffitient;
            var height = image.Height / koeffitient;
            var size = new Size((int) width, (int) height);
            return new Bitmap(image, size);
        }

        private void ToGrayScale()
        {
            var grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            RecogTools.ImageForRecognize = grayFilter.Apply(RecogTools.ImageForRecognize);
        }

        private void CorrectColorLevels()
        {
          
            var filter = new BradleyLocalThresholding();
         
            filter.ApplyInPlace(RecogTools.ImageForRecognize);


        }

        private void RemoveSkew()
        {
            //check the skew angle
            var skewChecker = new DocumentSkewChecker {MaxSkewToDetect = maxSkewRemovalAngle};
            var angle = skewChecker.GetSkewAngle(RecogTools.ImageForRecognize);

            //rotate image
            if (System.Math.Abs(angle) <= maxSkewRemovalAngle)
            {
                var rotationFilter = new RotateBilinear(-angle, true) {FillColor = Color.Black};
                RecogTools.ImageForRecognize = rotationFilter.Apply(RecogTools.ImageForRecognize);
            }
        }

        private Bitmap InvertImage(Bitmap image)
        {
            var invertfilter = new Invert();
            return invertfilter.Apply(image);
        }

        private BarCodeMarker FindBarCodeMarker()
        {
            BarCodeMarker result = null;
            //invert image before blob processing
            RecogTools.InvertedImage = InvertImage(RecogTools.ImageForRecognize);

            //lock image in memory
            var bitmapData = RecogTools.InvertedImage.LockBits(
                new Rectangle(0, 0, RecogTools.InvertedImage.Width, RecogTools.InvertedImage.Height), ImageLockMode.ReadWrite,
                RecogTools.InvertedImage.PixelFormat);

            //set filter for markers search 
            var blobCounter = new BlobCounter
            {
                FilterBlobs = true,
                MinHeight = UnitConverter.MmToPx(Properties.MarkerHeight - Properties.MarkerHeightTolerance,
                    RecogTools.InvertedImage.VerticalResolution),
                MaxHeight = UnitConverter.MmToPx(Properties.MarkerHeight + Properties.MarkerHeightTolerance,
                    RecogTools.InvertedImage.VerticalResolution),
                MinWidth =
                    UnitConverter.MmToPx(Properties.MarkerWidth - Properties.MarkerWidthTolerance,
                        RecogTools.InvertedImage.VerticalResolution),
                MaxWidth = UnitConverter.MmToPx(Properties.MarkerWidth + Properties.MarkerWidthTolerance,
                    RecogTools.InvertedImage.VerticalResolution)
            };



            //search markers
            blobCounter.ProcessImage(bitmapData);
            var blobs = blobCounter.GetObjectsInformation();
            RecogTools.InvertedImage.UnlockBits(bitmapData);


            if (blobs.Any())
            {
             
                foreach (var blob in blobs)
                {
                    
                        var findedMarker = new BarCodeMarker(blob, RecogTools.ImageForRecognize);

                        if (!string.IsNullOrEmpty(findedMarker.BarCodeContext))
                        {
                            result = findedMarker;
                        
                            break;
                        }
                     
                }
            }
            else
            {
                throw new FormRecognizeException(
                    $"Barcode marker not found. Blank cannot be recognized. Try to place the form correctrly");
            }
            if (result == null)
                throw new FormRecognizeException(
                    $"Barcode marker not found. Blank cannot be recognized. Try to place the form correctrly");

            return result;
        }

        #endregion
     
        #region Add Question

        public Question AddQuestion(Type questionType, string label, RecogMachine recogMachine)
        {
            if (RecogTools?.Marker == null)
            {
                throw new FormRecognizeException("Marker must be founded before question add");
            }
            
            var question = QuestionsDictionary.GetQuestion(questionType, label, RecogTools);

            question.SetRecogMachine(recogMachine);
            
            questions.Add(question);
            question.Index = questions.IndexOf(question);
            OnQuestionAdded(new QuestionEventArgs(question));
            return question;
        }

        public Question AddQuestion(Type questionType, string label, Guid guid, RecogMachine recogMachine)
        {
            if (RecogTools?.Marker == null)
            {
                throw new FormRecognizeException("Marker must be founded before question add");
            }


            var question = QuestionsDictionary.GetQuestion(questionType, label, guid, RecogTools);

            question.SetRecogMachine(recogMachine);
            questions.Add(question);
            question.Index = questions.IndexOf(question);
            OnQuestionAdded(new QuestionEventArgs(question));
            return question;
        }

        internal void AddQuestion(Question question)
        {
           questions.Add(question);
           OnQuestionAdded(new QuestionEventArgs(question));
        }


        #endregion

        public void RemoveQuestion(Question question)
        {
            //cells remove
            var cellCopyList = question.Cells.Select(c => new CheckCell(c.Guid, c.RecogTools,c.RecogMachine)).ToList();
            foreach (var cell in cellCopyList)
            {
                question.RemoveCell(cell);
            }
            questions.Remove(question);
            OnQuestionRemoved(new QuestionEventArgs(question));
        }

        protected virtual void OnQuestionAdded(QuestionEventArgs args)
        {
            QuestionAdded?.Invoke(this,args);
        }

        protected virtual void OnQuestionRemoved(QuestionEventArgs args)
        {
            QuestionRemoved?.Invoke(this,args);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Write & Read

        public RecogFormWrapper Wrap()
        {
            var adapter = new RecogFormWrapper
            {
                RecogTools = RecogTools.Wrap(),
                Properties = Properties,
                Questions = questions.Select(q => q.Wrap()).ToList()
            };
            return adapter;
        }

        #endregion

        public bool IsAllCellsFounded()
        {
            var result = true;
            foreach (var question in Questions)
            {
                foreach (var cell in question.Cells)
                {
                    if (cell.DistanceFromMarker == null)
                    {
                        return false;
                    }
                }
            }
            return result;
        }

    }
}