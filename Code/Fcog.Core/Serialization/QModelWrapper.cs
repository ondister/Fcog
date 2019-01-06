using System.Collections.Generic;
using System.Runtime.Serialization;
using Fcog.Core.Forms;

namespace Fcog.Core.Serialization
{
    [DataContract]
    public class QModelWrapper:IUnWrapped<QRecog>
    {
        [DataMember]
        public List<RecogFormWrapper> RecogForms { get; set; }

        [DataMember]
        public QuestionnareProperties Properties { get; set; }

        public QRecog UnWrap()
        {
           var result= new QRecog(Properties);
            foreach (var form in RecogForms)
            {
                var unwrappedForm = form.UnWrap();
                result.AddRecogForm(unwrappedForm);
            }


            return result;
        }
    }
}