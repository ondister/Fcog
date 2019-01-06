
namespace Fcog.Core.Serialization
{
   public interface IWrapped<out TWrapper>
   {
       TWrapper Wrap();
    }
}
