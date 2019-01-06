using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ConvNetSharp.Core;
using ConvNetSharp.Core.Layers.Double;
using ConvNetSharp.Volume;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Recognition
{
    public class RecogMachine : IWrapped<RecogMachineWrapper>
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
                TrainResult=TrainResult
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

        public Character FindCharacter(byte index)
        {
            return characters.FirstOrDefault(c => c.Index == index);
        }

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
            ConvolutionNet = new Net<double>();
            ConvolutionNet.AddLayer(new InputLayer(28, 28, 1));
            ConvolutionNet.AddLayer(new ConvLayer(5, 5, 8) {Stride = 1, Pad = 2});
            ConvolutionNet.AddLayer(new ReluLayer());
            ConvolutionNet.AddLayer(new PoolLayer(2, 2) {Stride = 2});
            ConvolutionNet.AddLayer(new ConvLayer(5, 5, 16) {Stride = 1, Pad = 2});
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

        public CellContent Recognize(Bitmap cellBitmap)
        {
            throw new NotImplementedException();
        }

       

        public override string ToString()
        {
            return Name;
        }

        #region Constructors

        public RecogMachine()
        {
            characters = new Collection<Character>();
            Characters = new ReadOnlyCollection<Character>(characters);
            Id = Guid.NewGuid();
            TrainResult=new TrainResult();
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

        private bool training;

        public TrainResult TrainResult { get; internal set; }
        
        public async Task StartTrainAsync(TrainerType trainerType, int batchSize, int maxEpochs)
        {
#warning сдедать проверку на инициализацию и наличие необходимого количества данных, совпадение данных и  символов машины
            TrainResult = new TrainResult();
            training = true;

            //reset net by create new
            CreateNet();

            var trainer = TrainersFactory.CreateTrainer(trainerType, batchSize, ConvolutionNet);
            if (trainer != null)
            {
                await Task.Run(() =>
                {
#warning возможно нужно перемешать

                    var trainAccuracyBuffer = new CircularBuffer<double>(100);
                    var testAccuracyBuffer = new CircularBuffer<double>(100);
                    for (var epoch = 1; epoch <= maxEpochs; epoch++)
                    {
                        var trainSample = DataSets.TrainDataSet.NextBatch(batchSize);
                        var testSample = DataSets.TestDataSet.NextBatch(batchSize);

                        #region Train
                      
                        trainer.Train(trainSample.InputVolume, trainSample.OutputVolume);
                        Test(trainSample.InputVolume, trainSample.Characters, trainAccuracyBuffer, false);
                        TrainResult.Loss = trainer.Loss;
                        TrainResult.TrainAccuracy = System.Math.Round(trainAccuracyBuffer.Items.Average() * 100.0, 2);

                        #endregion

                        #region Test

                        Test(testSample.InputVolume, testSample.Characters, testAccuracyBuffer, true);
                        TrainResult.TestAccuracy = System.Math.Round(testAccuracyBuffer.Items.Average() * 100.0, 2);

                        #endregion

                        TrainResult.EpochsCount = epoch;

                        OnEpochDone();

                        if (!training)
                        {
                            break;
                        }
                    }
                });
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
            
            for (var i = 0; i < labels.Count; i++)
            {
                accuracy.Add(labels[i].Index == prediction[i] ? 1.0 : 0.0);
            }
        }


        public void StopTrain()
        {
            training = false;
        }

        #endregion

        protected virtual void OnEpochDone()
        {
            EpochDone?.Invoke(this, EventArgs.Empty);
        }
    }
}