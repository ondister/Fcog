using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Fcog.Core.Recognition;

namespace Fcog.Core.Serialization.Recognition
{
    [DataContract]
    public class RecogMachineWrapper:IUnWrapped<RecogMachine>
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<CharacterWrapper> Characters { get; set; }

        [DataMember]
        public TrainResult TrainResult {get;set;}

        public RecogMachine UnWrap()
        {
            var result = new RecogMachine(Name, Id) {TrainResult = TrainResult};

            foreach (var character in Characters)
            {
                result.AddCharacter(character.Index,character.TextView);
            }


            return result;
        }
    }
}