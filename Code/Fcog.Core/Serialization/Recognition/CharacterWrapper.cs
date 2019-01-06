using System.Runtime.Serialization;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;

namespace Fcog.Core.Serialization.Recognition
{
    [DataContract]
   public class CharacterWrapper:IUnWrapped<Character>
    {
        [DataMember]
        public byte Index { get; set; }

        [DataMember]
        public TextView TextView { get; set; }

        public Character UnWrap()
        {
            return new Character(Index, TextView);
        }
    }
}