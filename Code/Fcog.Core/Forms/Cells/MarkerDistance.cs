using System.Runtime.Serialization;

namespace Fcog.Core.Forms.Cells
{
    [DataContract]
    public class MarkerDistance
    {
        public MarkerDistance(int left, int top)
        {
            Left = left;
            Top = top;
        }

        [DataMember]
        public int Left { get; set; }

        [DataMember]
        public int Top { get; set; }
    }
}