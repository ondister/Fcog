using System.Runtime.Serialization;
using Fcog.Core.Forms;

namespace Fcog.Core.Serialization
{
    [DataContract]
    public class RecogToolsWrapper:IUnWrapped<RecogTools>
    {
        [DataMember]
        public BarCodeMarkerWrapper Marker { get; set; }

        public RecogTools UnWrap()
        {
            var result = new RecogTools {Marker = Marker.UnWrap()};
            return result;
        }
    }
}