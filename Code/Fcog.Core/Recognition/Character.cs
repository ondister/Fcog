using System;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Recognition
{
    public class Character : IEquatable<Character>, IWrapped<CharacterWrapper>
    {
        public Character(byte index, TextView textView)
        {
            Index = index;
            TextView = textView;
        }

        public byte Index { get; }


        public TextView TextView { get; }

        public bool Equals(Character other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Index == other.Index && Equals(TextView, other.TextView);
        }

        public CharacterWrapper Wrap()
        {
            return new CharacterWrapper {Index = Index, TextView = TextView};
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Character) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Index * 397) ^ (TextView != null ? TextView.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{Index}, {TextView}";
        }
    }
}