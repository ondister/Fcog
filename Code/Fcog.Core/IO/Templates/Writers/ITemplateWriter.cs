using Fcog.Core.Serialization;

namespace Fcog.Core.IO.Templates.Writers
{
    public interface ITemplateWriter
    {
        WriteResult Write(QModelWrapper questionnaire);
    }
}
