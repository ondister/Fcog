using System;
using System.Linq;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Localization;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    /// <summary>
    ///     The answer is unrecognizable handwriting.
    /// </summary>
    public sealed class TextQuestion : Question
    {
        public static string TypeDescription => CoreUI.QuestionTypeTextSign;

        public override QuestionWrapper Wrap()
        {
            var adapter = new TextQuestionWrapper
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
            //clear cell
            if (Cells.Any())
            {
                RemoveCell(Cells.Single());
            }

            var cell = new TextCell(RecogTools, RecogMachine)
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
        //    //clear cell
        //    if (Cells.Any())
        //    {
        //        RemoveCell(Cells.Single());
        //    }

        //    var cell = new TextCell(guid, RecogTools,recogMachine)
        //    {
        //        Label = cellLabel
        //    };
        //    cells.Add(cell);
        //    cell.Index =cells.IndexOf(cell);
        //    OnCellAdded(new CellEventArgs(cell));
        //    return cell;
        //}


        internal override void AddCell(Cell cell)
        {
            //clear cell
            if (Cells.Any())
            {
                RemoveCell(Cells.Single());
            }

            cells.Add(cell);
            OnCellAdded(new CellEventArgs(cell));
        }


        #region Constructors

        internal TextQuestion(string label, RecogTools recogTools) : base(label, recogTools)
        {
            AddCell(Label);
        }

        internal TextQuestion(string label, Guid guid, RecogTools recogTools) : base(label, guid, recogTools)
        {
        }

        #endregion
    }
}