using System.Collections.Generic;
using System.Runtime.Serialization;
using Fcog.Core.Forms;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Serialization
{
    [DataContract]
    public class RecogFormWrapper:IUnWrapped<RecogForm>
    {
        [DataMember]
        public RecogFormProperties Properties { get; set; }

        [DataMember]
        public List<QuestionWrapper> Questions { get; set; }

        [DataMember]
        public RecogToolsWrapper RecogTools { get; set; }

        public RecogForm UnWrap()
        {
            var result = new RecogForm(Properties, WorkMode.Unknown) {RecogTools = RecogTools.UnWrap()};

            foreach (var question in Questions)
            {
                var unWrappedQuestion = question.UnWrap();

                result.AddQuestion(unWrappedQuestion);
              
            }


            return result;
        }
    }
}