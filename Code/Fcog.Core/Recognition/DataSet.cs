using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ConvNetSharp.Volume;
using ConvNetSharp.Volume.Double;

namespace Fcog.Core.Recognition
{
    public class DataSet
    {
        
        public static int ImageWidth => 28;
        public static int ImageHeight => 28;




        private const int inputDimention = 1;
        private const int outputWeight = 1;
        private const int outputHeight= 1;

        private const double maxGrayValue = 255.0;

        private readonly Random random = new Random(RandomUtilities.Seed);
        private int dataIndex;
     
        private readonly IList<DataSetPair> dataSetPairs;
        private readonly IList<Character> characters;

        public DataSet(IList<DataSetPair> dataSetPairs, IList<Character> characters)
        {
            this.dataSetPairs= dataSetPairs;
            this.characters = characters;
            DataSetPairs= new ReadOnlyCollection<DataSetPair>(dataSetPairs);

       }

        public int Count => dataSetPairs.Count;

        public ReadOnlyCollection<DataSetPair> DataSetPairs { get; }

       public void ShuffleData()
        {
            for (var index = dataSetPairs.Count - 1; index >= 0; index--)
            {
                var randomIndex = random.Next(index);
                var tempPair = dataSetPairs[randomIndex];
                dataSetPairs[randomIndex] = dataSetPairs[index];
                dataSetPairs[index] = tempPair;
            }
        }

        public TrainBatch NextBatch(int batchSize)
        {
         
            var classesCount = characters.Count;

            var imageShape = new Shape(ImageWidth, ImageHeight, inputDimention, batchSize);
            var outputShape = new Shape(outputWeight, outputHeight, classesCount, batchSize);
            var imageData = new double[imageShape.TotalLength];
            var outputData = new double[outputShape.TotalLength];
            var charactersBatch = new List<Character>(batchSize);

            var inputVolume = BuilderInstance.Volume.From(imageData, imageShape);

            for (var batchIndex = 0; batchIndex < batchSize; batchIndex++)
            {
#warning add try catch for autofrange exception
                var entry = dataSetPairs[dataIndex];

                charactersBatch.Add(entry.Character);

                var pixelIndex = 0;
                for (var heightIndex = 0; heightIndex < ImageHeight; heightIndex++)
                {
                    for (var widthIndex = 0; widthIndex < ImageWidth; widthIndex++)
                    {
                        inputVolume.Set(widthIndex, heightIndex, 0, batchIndex, entry.ImageBytes[pixelIndex++] / maxGrayValue);
                    }
                }

                outputData[batchIndex * classesCount + entry.Character.Index] = 1.0;

                dataIndex++;

                if (dataIndex == dataSetPairs.Count)
                {
                    dataIndex = 0;
                }
            }


            var outputVolume = BuilderInstance.Volume.From(outputData, outputShape);

            return new TrainBatch(inputVolume,outputVolume,charactersBatch);
        }


       public void AddDataSetPair(DataSetPair pair)
        {
            dataSetPairs.Add(pair);
        }

        public DataSetStatistics GetStatistics()
        {
          
            var datasetPairscount = dataSetPairs.Count;
            var result = new DataSetStatistics(datasetPairscount);

            foreach (var character in characters)
            {
                var charactersCount = dataSetPairs.Count(p => p.Character.Equals(character));
                var frequency = 0.0;

                if (datasetPairscount != 0)
                {
                  frequency = (double) charactersCount / datasetPairscount;
                }

                var characterStatistics= new CharacterStatistics(character,frequency, charactersCount);
                result.AddCharacterStatistics(characterStatistics);
            }

            return result;
        }

    }
}