using System;
using System.Linq;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Localization;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    /// <summary>
    ///     The question contains several correct answers.
    /// </summary>
    public sealed class MultiQuestion : Question
    {
        public static string TypeDescription => CoreUI.QuestionTypeMultiSign;
        public override QuestionWrapper Wrap()
        {
            var adapter = new MultiQuestionWrapper
            {
                Guid = Guid,
                Cells = Cells.Select(c => c.Wrap()).ToList(),
                Index = Index,
                Label = Label,
                RecogTools = RecogTools.Wrap(),
                RecogMachine = recogMachine.Wrap()
            };

            return adapter;
        }


        public override Cell AddCell(string cellLabel)
        {
            var cell = new CheckCell(RecogTools, recogMachine)
            {
                Label = cellLabel
            };
            cells.Add(cell);
            cell.Index = cells.IndexOf(cell);
            OnCellAdded(new CellEventArgs(cell));
            return cell;
        }

        //public Cell AddCell(Guid cellGuid, string cellLabel)
        //{
        //    var cell = new CheckCell(cellGuid, RecogTools,recogMachine)
        //    {
        //        Label = cellLabel,
        //    };
        //    cells.Add(cell);
        //    cell.Index = Cells.IndexOf(cell);
        //    OnCellAdded(new CellEventArgs(cell));
        //    return cell;

        //}

        internal override void AddCell(Cell cell)
        {
            cells.Add(cell);
            OnCellAdded(new CellEventArgs(cell));
        }


        #region Constructors

        internal MultiQuestion(string label, RecogTools recogTools) : base(label, recogTools)
        {
        }

        internal MultiQuestion(string label, Guid guid, RecogTools recogTools) : base(label, guid, recogTools)
        {
        }

        #endregion
    }
}