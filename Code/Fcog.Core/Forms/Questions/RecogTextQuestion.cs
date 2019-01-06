using System;
using System.Linq;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Localization;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    /// <summary>
    //The answer is recognizable handwriting.
    /// </summary>
    public sealed class RecogTextQuestion : Question
    {
        public static string TypeDescription => CoreUI.QuestionTypeRecogTextSign;
        public override QuestionWrapper Wrap()
        {
            var adapter = new RecogTextQuestionWrapper
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
            var cell = new LetterCell(RecogTools, recogMachine)
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
        //    var cell = new LetterCell(cellGuid, RecogTools,recogMachine)
        //    {
        //        Label = cellLabel
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

        internal RecogTextQuestion(string label, RecogTools recogTools) : base(label, recogTools)
        {
        }

        internal RecogTextQuestion(string label, Guid guid, RecogTools recogTools) : base(label, guid, recogTools)
        {
        }

        #endregion
    }
}