using System;

using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization.Cells;

namespace Fcog.Core.Forms.Cells
{
    /// <summary>
    ///     Cell containing a recognizable letter.
    /// </summary>
    public class LetterCell : Cell
    {

        internal LetterCell(RecogTools recogTools, RecogMachine recogMachine) : base(recogTools, recogMachine)
        {

            Content = new CellContent();
        }

        internal LetterCell(Guid guid, RecogTools recogTools, RecogMachine recogMachine) : base(guid, recogTools, recogMachine)
        {
            Content = new CellContent();
        }


        
        public override CellWrapper Wrap()
        {
            var wraper = new LetterCellWrapper
            {
                RecogTools = RecogTools.Wrap(),
                Content = Content,
                DistanceFromMarker = DistanceFromMarker,
                Guid = Guid,
                Index = Index,
                Label = Label,
                Rectangle = Rectangle,
                RecogMachine= RecogMachine.Wrap()
            };
            return wraper;
        }
    }
}