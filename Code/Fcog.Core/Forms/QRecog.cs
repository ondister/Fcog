using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fcog.Core.IO.Templates.Readers;

namespace Fcog.Core.Forms
{
    public class QRecog:Questionnaire
    {

        public QRecog(QuestionnareProperties properties)
        {
            this.properties = properties;
        }

        internal void AddRecogForm(RecogForm recogForm)
        {
            recogForms.Add(recogForm);
        }

        public static QRecog OpenFromTemplate(ITemplateReader templatereader, Guid questionnaireGuid)
        {
            var readResult = templatereader.Read(questionnaireGuid);

            return readResult.Result?.UnWrap();

        }

    }
}
