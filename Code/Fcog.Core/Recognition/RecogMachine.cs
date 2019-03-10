using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Accord.Imaging.Filters;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;
using Fcog.Core.Annotations;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Recognition
{
    public class RecogMachine : IWrapped<RecogMachineWrapper>,INotifyPropertyChanged
    {
        private readonly Collection<Character> characters;
        

        public Guid Id { get; }
        public string Name { get; set; }

        public ReadOnlyCollection<Character> Characters { get; }

        public DataSets DataSets { get; private set; }

        public Net<double> ConvolutionNet { get; private set; }

        public bool Initialized => ConvolutionNet != null && DataSets != null;

        public RecogMachineWrapper Wrap()
        {
            var result = new RecogMachineWrapper
            {
                Id = Id,
                Characters = characters.Select(c => new CharacterWrapper {Index = c.Index, TextView = c.TextView}),
                Name = Name,
                TrainResult = TrainResult
            };
            return result;
        }

        public void AddCharacter(byte index, TextView textView)
        {
            var character = new Character(index, textView);
            if (!characters.Contains(character))
            {
                characters.Add(character);
            }
        }

        public Character FindCharacter(int index)
        {
            return characters.FirstOrDefault(c => c.Index == index);
        }

        public Character FindCharacter(TextView textView)
        {
            return characters.FirstOrDefault(c => c.TextView.Equals(textView));
        }

        #region Initialize

        public void Initialize()
        {
            if (!Initialized)
            {
                CreateNet();
                CreateDataSets();
            }
        }

        internal void Initialize(Net<double> net, DataSets dataSets)
        {
            ConvolutionNet = net;
            DataSets = dataSets;
        }

        private void CreateNet()
        {
            const int dimention = 1;//grayScale images

            ConvolutionNet = new Net<double>();
            ConvolutionNet.AddLayer(new InputLayer(DataSet.ImageWidth, DataSet.ImageHeight, dimention));
            ConvolutionNet.AddLayer(new ConvLayer(3, 3, 8) {Stride = 1, Pad = 2});
            ConvolutionNet.AddLayer(new ReluLayer());
            ConvolutionNet.AddLayer(new PoolLayer(2, 2) {Stride = 2});
            ConvolutionNet.AddLayer(new ConvLayer(3, 3, 16) {Stride = 1, Pad = 2});
            ConvolutionNet.AddLayer(new ReluLayer());
            ConvolutionNet.AddLayer(new PoolLayer(3, 3) {Stride = 3});
            ConvolutionNet.AddLayer(new FullyConnLayer(characters.Count));
            ConvolutionNet.AddLayer(new SoftmaxLayer(characters.Count));
        }
        
        private void CreateDataSets()
        {
            var trainSet = new DataSet(new List<DataSetPair>(), Characters);
            var testSet = new DataSet(new List<DataSetPair>(), Characters);
            DataSets = new DataSets(trainSet, testSet);
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }



        #region Recognition
        public async Task<CellContent> RecognizeAsync(Bitmap cellBitmap)
        {
            CellContent result = null;

            await Task.Run(() =>
          {
             result= Recognize(cellBitmap);
            
          });

            return result;
        }

        public CellContent Recognize(Bitmap cellBitmap)
        {
            if (!Initialized)
            {
                throw new Exception("Net is not initialized");
            }

            var content = new CellContent();

            #region Prepare data

            const int fillThickness = 5; //you may need to decrease this value if the cell is clipped severely.
            var clearedBitmap = ClearBitmap(cellBitmap, fillThickness);
            var resizedBitmap = ResizeBitmap(clearedBitmap);


            var imageBytes = ConvertBitmap(resizedBitmap);

#if DEBUG
            Directory.CreateDirectory("images");
            var dataPair= new DataSetPair(imageBytes,new Character(0,TextViews.Empty));
            dataPair.Save($"images");
#endif


            var inputData = CreateInputData(imageBytes);

#endregion

            ConvolutionNet.Forward(inputData);
            var prediction = ConvolutionNet.GetPrediction();

            var textView = FindCharacter(prediction[0])?.TextView;

            content.TextView = textView;
            content.ImageBytes = imageBytes;

            return content;
        }

        public Bitmap ResizeBitmap(Bitmap image)
        {
            var containerHeight = DataSet.ImageHeight;
            var containerWidth = DataSet.ImageWidth;

            var scaleWidth = (double) containerWidth / image.Width;
            var scaleHeight = (double) containerHeight / image.Height;
            var scale = Math.Min(scaleWidth, scaleHeight);

            var scaledWidth = (int) (image.Width * scale);
            var scaledHeight = (int) (image.Height * scale);

            var containerImage = new Bitmap(containerWidth, containerHeight);
            containerImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(containerImage))
            {
                var centerX = containerWidth / 2 - scaledWidth / 2;
                var centerY = containerHeight / 2 - scaledHeight / 2;

                graphics.DrawImage(image, centerX, centerY, scaledWidth, scaledHeight);
            }

            return containerImage;
        }

        private Bitmap ClearBitmap(Bitmap bitmap, int fillThickness)
        {
            const byte fillColor = 0;
            var fillRectangle = new Rectangle(fillThickness, fillThickness, bitmap.Width - fillThickness * 2,
                bitmap.Height - fillThickness * 2);

            //The filter fills areas outside of specified region using the specified color.
            var cropFilter = new CanvasCrop(fillRectangle, fillColor);

            return cropFilter.Apply(bitmap);
        }

        private byte[] ConvertBitmap(Bitmap bitmap)
        {
            var imageBytes = new byte [bitmap.Height * bitmap.Width];
            var imageBytesIndex = 0;
            const byte blackPixelByte = 0;

            for (var yIndex = 0; yIndex < bitmap.Width; yIndex++)
            {
                for (var xIndex = 0; xIndex < bitmap.Height; xIndex++)
                {
                    var pixelColor = bitmap.GetPixel(xIndex, yIndex);
                    var pixelGrayScale = pixelColor.R + pixelColor.G + pixelColor.B;

                    if (pixelColor.A == 0) //if pixel is transarent
                    {
                        pixelGrayScale = blackPixelByte;
                    }

                    //find average of color
                    imageBytes[imageBytesIndex] = (byte) (pixelGrayScale / 3);

                    imageBytesIndex++;
                }
            }

            return imageBytes;
        }

        private Volume<double> CreateInputData(IReadOnlyList<byte> imageBytes)
        {
            const int inputDimention = 1;
            const int imageCount = 1;
            const double maxGrayValue = 255.0;
            const int batchIndex = 0;

            var imageShape = new Shape(DataSet.ImageWidth, DataSet.ImageHeight, inputDimention, imageCount);
            var imageData = new double[imageShape.TotalLength];

            var inputVolume = BuilderInstance.Volume.From(imageData, imageShape);

            var pixelIndex = 0;
            for (var heightIndex = 0; heightIndex < DataSet.ImageHeight; heightIndex++)
            {
                for (var widthIndex = 0; widthIndex < DataSet.ImageWidth; widthIndex++)
                {
                    inputVolume.Set(widthIndex, heightIndex, 0, batchIndex, imageBytes[pixelIndex++] / maxGrayValue);
                }
            }

            return inputVolume;
        }

#endregion

#region Constructors

        public RecogMachine()
        {
            characters = new Collection<Character>();
            Characters = new ReadOnlyCollection<Character>(characters);
            Id = Guid.NewGuid();
            TrainResult = new TrainResult();
        }

        public RecogMachine(string name) : this()
        {
            Name = name;
        }

        internal RecogMachine(string name, Guid id) : this(name)
        {
            Name = name;
            Id = id;
        }

#endregion
        
#region Train

        public event EventHandler EpochDone;
        protected virtual void OnEpochDone()
        {
            EpochDone?.Invoke(this, EventArgs.Empty);
        }

        private bool training;
        public bool Training
        {
            get => training;
            private set
            {
                if (value == training) return;
                training = value;
                if (cancellationTokenSource != null && value == false) 
                {
                  cancellationTokenSource.Cancel();
                }

                OnPropertyChanged();
            }
        }

        private CancellationTokenSource cancellationTokenSource; 

        public TrainResult TrainResult { get; internal set; }

        public async Task StartTrainAsync(TrainerType trainerType, int batchSize, int maxEpochs, double minTestAcuracy=1)
        {
#warning сдедать проверку на инициализацию и наличие необходимого количества данных, совпадение данных и  символов машины
           TrainResult = new TrainResult();
           Training = true;

            cancellationTokenSource= new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            //reset net by create new
            CreateNet();

            var trainer = TrainersFactory.CreateTrainer(trainerType, batchSize, ConvolutionNet);
            if (trainer != null)
            {
                await Task.Run(() =>
                {
#warning возможно нужно перемешать
                    const int minBufferLenght = 100;
                    var bufferSize = batchSize >= minBufferLenght ? batchSize : minBufferLenght;

                    var trainAccuracyBuffer = new CircularBuffer<double>(bufferSize);
                    var testAccuracyBuffer = new CircularBuffer<double>(bufferSize);
                    for (var epoch = 1; epoch <= maxEpochs; epoch++)
                    {
                        var trainSample = DataSets.TrainDataSet.NextBatch(batchSize);
                        var testSample = DataSets.TestDataSet.NextBatch(batchSize);

#region Train

                        trainer.Train(trainSample.InputVolume, trainSample.OutputVolume);
                        Test(trainSample.InputVolume, trainSample.Characters, trainAccuracyBuffer, false);
                        TrainResult.Loss = trainer.Loss;
                      
                        TrainResult.TrainAccuracy = Math.Round(trainAccuracyBuffer.Items.Average(), 2);


#endregion

#region Test

                        Test(testSample.InputVolume, testSample.Characters, testAccuracyBuffer, true);
                        TrainResult.TestAccuracy = Math.Round(testAccuracyBuffer.Items.Average(), 2);

#endregion

                        TrainResult.EpochsCount = epoch;

                        OnEpochDone();

                        //stop training conditions
                        if (TrainResult.TestAccuracy >= minTestAcuracy)
                        {
                           StopTrain();
                        }

                        if (cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }, token);
            }
        }


        private void Test(Volume<double> inputVolume, IList<Character> labels, CircularBuffer<double> accuracy,
            bool forward)
        {
            if (forward)
            {
                ConvolutionNet.Forward(inputVolume);
            }

            var prediction = ConvolutionNet.GetPrediction();

            if (prediction == null)
            {
                throw new Exception();
            }

            for (var i = 0; i < labels.Count; i++)
            {
                accuracy.Add(labels[i].Index == prediction[i] ? 1.0 : 0.0);
            }
        }


        public void StopTrain()
        {
            Training = false;
        }

#endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      
    }
}