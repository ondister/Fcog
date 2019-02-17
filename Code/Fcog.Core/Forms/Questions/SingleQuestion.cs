using System;
using System.Linq;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Localization;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    /// <summary>
    ///     The question contains one correct answer.
    /// </summary>
    public sealed class SingleQuestion : Question
    {
        public static string TypeDescription => CoreUI.QuestionTypeSingleSign;


        public override QuestionWrapper Wrap()
        {
            var adapter = new SingleQuestionWrapper
            {
                Guid = Guid,
                Cells = Cells.Select(c => c.Wrap()).ToList(),
                Index = Index,
                Label = Label,
                RecogTools = RecogTools.Wrap(),
                RecogMachine = RecogMachine.Wrap()
            };

            return adapter;
        }


        public override Cell AddCell(string cellLabel)
        {
            var cell = new RadioCell(RecogTools, RecogMachine)
            {
                Label = cellLabel,
                GroupName = Guid.ToString()
            };
            cells.Add(cell);
            cell.Index = cells.IndexOf(cell);
            OnCellAdded(new CellEventArgs(cell));
            return cell;
        }

        //public Cell AddCell(Guid cellGuid, string cellLabel)
        //{
        //    var cell = new RadioCell(cellGuid, RecogTools, recogMachine)
        //    {
        //        Label = cellLabel,
        //        GroupName = Guid.ToString()
        //    };
        //    cells.Add(cell);
        //    cell.Index = cells.IndexOf(cell);
        //    OnCellAdded(new CellEventArgs(cell));
        //    return cell;
        //}

        internal override void AddCell(Cell cell)
        {
            cells.Add(cell);
            OnCellAdded(new CellEventArgs(cell));
        }


        #region Constructors

        internal SingleQuestion(string label, RecogTools recogTools) : base(label, recogTools)
        {
        }

        internal SingleQuestion(string label, Guid guid, RecogTools recogTools) : base(label, guid, recogTools)
        {
        }

        #endregion
    }
}