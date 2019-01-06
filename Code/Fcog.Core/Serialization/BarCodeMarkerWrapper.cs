using System.Runtime.Serialization;
using Accord;
using Fcog.Core.Barcodes;

namespace Fcog.Core.Serialization
{
    [DataContract]
    public class BarCodeMarkerWrapper:IUnWrapped<BarCodeMarker>
    {
        [DataMember]
        public string BarCodeContext { get; set; }

        [DataMember]
        public float CenterOfGravityX { get; set; }

        [DataMember]
        public float CenterOfGravityY { get; set; }

        public BarCodeMarker UnWrap()
        {
            var result= new BarCodeMarker(BarCodeContext,CenterOfGravityX,CenterOfGravityY);
         
            return result;
        }
    }
}