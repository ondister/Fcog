namespace Fcog.Core.Recognition
{
    public class DataSets
    {
        public DataSet TrainDataSet { get; }

        public DataSet TestDataSet { get; }


        public DataSets(DataSet trainDataSet, DataSet testDataSet)
        {
            TrainDataSet = trainDataSet;
            TestDataSet = testDataSet;
      }
    }
}
