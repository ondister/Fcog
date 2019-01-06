using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization.Cells;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Serialization.Questions
{
    [DataContract]
    [KnownType(typeof(MarkQuestionWrapper)),
     KnownType(typeof(MultiQuestionWrapper)),
     KnownType(typeof(RecogTextQuestionWrapper)),
     KnownType(typeof(SingleQuestionWrapper)),
     KnownType(typeof(TextQuestionWrapper))
        ]
    public abstract class QuestionWrapper:IUnWrapped<Question>
    {
       [DataMember]
       public RecogMachineWrapper RecogMachine{ get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public List<CellWrapper> Cells { get; set; }


        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public RecogToolsWrapper RecogTools { get; set; }

        public abstract Question UnWrap();

    }
}