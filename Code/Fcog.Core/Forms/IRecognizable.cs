

using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Fcog.Core.Forms
{
    
   public interface IRecognizable
    {
       [DataMember]
       RecogTools RecogTools { get; set; }

        void Recognize();

        Task RecognizeAsync();
    }
}
