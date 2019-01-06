using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fcog.Core.IO;
using Fcog.Core.IO.Templates.Writers;
using Fcog.Core.Localization;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;

namespace Fcog.Core.Forms
{
  public  class QModel:Questionnaire,IWrapped<QModelWrapper>
    {

        public QModel(QuestionnareProperties properties)
        {
            this.properties = properties;
        }

        public QModel(string name, DateTime creationDateTime)
        {
            Properties.Name = name;
            Properties.CreationDateTime = creationDateTime;
        }


        public RecogForm AddRecogForm(RecogFormProperties recogFormProperties)
        {
            RecogForm result = null;
            if (recogFormProperties != null)
            {
                var recogForm = new RecogForm(recogFormProperties, WorkMode.CreateForm);
                recogForms.Add(recogForm);
                result = recogForm;
            }

            return result;
        }

        public QModelWrapper Wrap()
        {
            var result = new QModelWrapper
            {
                Properties = Properties,
                RecogForms = recogForms.Select(r => r.Wrap()).ToList()
            };
            return result;
        }

        public WriteResult SaveTemplate(ITemplateWriter templateWriter)
        {
            if (!IsQuesionnaireValid())
            {
                throw new FormRecognizeException(CoreUI.ThrowQuestionnaireNotValid);
            }
            CheckProperties();
            var wrapper = Wrap();
            return templateWriter.Write(wrapper);
        }

        private void CheckProperties()
        {
            //check the guid before save
            if (Properties.Guid == Guid.Empty) Properties.Guid = Guid.NewGuid();
            //check the creation date before save
            if (!Properties.CreationDateTime.HasValue) Properties.CreationDateTime = DateTime.Now;
        }

        private bool IsQuesionnaireValid()
        {
            var result = true;
            foreach (var form in RecogForms)
            {
                if (!form.Questions.Any() || !form.IsAllCellsFounded())
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
