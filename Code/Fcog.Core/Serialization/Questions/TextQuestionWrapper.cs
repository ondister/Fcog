using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fcog.Core.Forms.Questions;
using Fcog.Core.Recognition;

namespace Fcog.Core.Serialization.Questions
{
   public class TextQuestionWrapper:QuestionWrapper
    {
        public override Question UnWrap()
        {
            var result = new TextQuestion(Label, Guid, RecogTools.UnWrap()) { Index = Index };


            var recogMachine = RecogMachine.UnWrap();
            recogMachine = RecogMachinesPool.Instance.GetRecogMachine(recogMachine.Id);
            result.SetRecogMachine(recogMachine);



            foreach (var cell in Cells)
            {
                var unWrappedCell = cell.UnWrap();
                unWrappedCell.SetRecogMachine(recogMachine);
                result.AddCell(unWrappedCell);
            }

            return result;
        }
    }
}
