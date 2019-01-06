
namespace Fcog.Core.Serialization
{
    public interface IUnWrapped<out TSource>
    {
        TSource UnWrap();
    }
}
