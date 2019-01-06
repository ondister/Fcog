using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConvNetSharp.Volume;

namespace Fcog.Core.Recognition
{
     public   class TrainBatch
    {
      public  Volume<double> InputVolume { get; }
      public Volume<double> OutputVolume { get; }
      public ReadOnlyCollection<Character> Characters { get; }

        public TrainBatch(Volume<double> inputVolume, Volume<double> outputVolume, IList<Character> characters)
        {
            InputVolume = inputVolume;
            OutputVolume = outputVolume;
            Characters = new ReadOnlyCollection<Character>(characters);
        }
    }
}
