using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Cells;

namespace Fcog.Core.Forms.Cells
{
    /// <summary>
    ///     Cell containing unrecognizable text.
    /// </summary>

    public class TextCell : Cell
    {
       internal TextCell(RecogTools recogTools, RecogMachine recogMachine) :base(recogTools,recogMachine)
        {
            Content = new CellContent();
        }

        internal TextCell(Guid guid, RecogTools recogTools, RecogMachine recogMachine) :base(guid,recogTools,recogMachine)
        {
            Content = new CellContent();
        }

        #region IWrapped


        public override CellWrapper Wrap()
        {
            var adapter = new TextCellWrapper
            {
                RecogTools = RecogTools.Wrap(),
                Content = Content,
                DistanceFromMarker = DistanceFromMarker,
                Guid = Guid,
                Index = Index,
                Label = Label,
                Rectangle = Rectangle,
                RecogMachine = RecogMachine.Wrap()
            };
            return adapter;
        }
        #endregion

        public override void Recognize()
        {
           
        }
    }
}