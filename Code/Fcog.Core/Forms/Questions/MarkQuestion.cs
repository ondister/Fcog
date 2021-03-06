﻿using System;
using System.Linq;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Localization;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization.Questions;

namespace Fcog.Core.Forms.Questions
{
    /// <summary>
    ///     The answer is a mark.
    /// </summary>
    public sealed class MarkQuestion : Question
    {
        public static string TypeDescription => CoreUI.QuestionTypeMarkSign;

        public override Cell AddCell(string cellLabel)
        {
            //clear  cell
            if (Cells.Any())
            {
                RemoveCell(Cells.Single());
            }

            var cell = new CheckCell(RecogTools, RecogMachine)
            {
                Label = cellLabel
            };


            cells.Add(cell);
            cell.Index = cells.IndexOf(cell);
            cell.Label = Label;
            OnCellAdded(new CellEventArgs(cell));
            return cell;
        }

        internal override void SetRecogMachine(RecogMachine recogMachine)
        {
            base.SetRecogMachine(recogMachine);
            foreach (var cell in Cells)
            {
                cell.SetRecogMachine(RecogMachine);
            }
        }

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

        public override QuestionWrapper Wrap()
        {
            var adapter = new MarkQuestionWrapper
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


        #region Constructors

        internal MarkQuestion(string label, RecogTools recogTools) : base(label, recogTools)
        {
            AddCell(Label);
        }

        internal MarkQuestion(string label, Guid guid, RecogTools recogTools) : base(label, guid, recogTools)
        {
        }

        #endregion

     
    }
}