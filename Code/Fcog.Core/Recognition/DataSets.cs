using System;
using System.Linq;
using Fcog.Core.Forms.Cells.Content;

namespace Fcog.Core.Recognition
{
    public class DataSets
    {
        public bool Changed { get; private set; }
        public DataSet TrainDataSet { get; }

        public DataSet TestDataSet { get; }


        public DataSets(DataSet trainDataSet, DataSet testDataSet)
        {
            TrainDataSet = trainDataSet;
            TestDataSet = testDataSet;
      }

        public void AddCellContent(CellContent cellContent)
        {
            const double countKoeff = 6;
            var trainStatistics = TrainDataSet.GetStatistics();
            var testStatistics = TestDataSet.GetStatistics();

            var characterTrainStat =
                trainStatistics.CharactersStatistics.FirstOrDefault(c =>
                    c.Character.TextView.Equals(cellContent.TextView));

            var characterTestStat =
                testStatistics.CharactersStatistics.FirstOrDefault(c =>
                    c.Character.TextView.Equals(cellContent.TextView));

            
            
            if (characterTrainStat != null && characterTestStat != null)
            {
                var dataPair = new DataSetPair(cellContent.ImageBytes, characterTrainStat.Character);

#if DEBUG
           dataPair.Save("savedImages");
#endif
#warning переписать к чертовой бабушке
                if (characterTrainStat.Count == 0 || characterTestStat.Count == 0)
                {
                    TrainDataSet.AddDataSetPair(dataPair);
                    TestDataSet.AddDataSetPair(dataPair);
                }
                else
                {
                    var realCountKoeff = characterTrainStat.Count / characterTestStat.Count;

                    if (realCountKoeff <= countKoeff)
                    {
                        TrainDataSet.AddDataSetPair(dataPair);
                    }
                    else
                    {
                        TestDataSet.AddDataSetPair(dataPair);
                    }
                }

                Changed = true;
            }
            else
            {
#warning replace the exception with derived class
                throw new Exception("no character in datasets");
            }
        }
    }
}
