using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace Fcog.Core.Recognition
{
    public class DataSetStatistics
    {
        private readonly ObservableCollection<CharacterStatistics> charactersStatistics;
        public DataSetStatistics(int datasetPairsCount)
        {
            charactersStatistics=new ObservableCollection<CharacterStatistics>();
            CharactersStatistics=new ReadOnlyObservableCollection<CharacterStatistics>(charactersStatistics);
            DatasetPairsCount = datasetPairsCount;
        }

        public int DatasetPairsCount { get; private set; }
        
        public ReadOnlyObservableCollection<CharacterStatistics> CharactersStatistics { get; }

        internal void AddCharacterStatistics(CharacterStatistics characterStatistics)
        {
            charactersStatistics.Add(characterStatistics);
        }


    }
}