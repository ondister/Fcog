using System;
using System.Runtime.Serialization;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization;
using Fcog.Core.Serialization.Cells;

namespace Fcog.Core.Forms.Cells
{
    /// <summary>
    ///     CheckBox cell.
    /// </summary>
    public class CheckCell : Cell
    {
       
        internal CheckCell(RecogTools recogTools, RecogMachine recogMachine):base(recogTools, recogMachine)
        {
            Content = new CellContent();
        }

        internal CheckCell(Guid guid, RecogTools recogTools, RecogMachine recogMachine) :base(guid, recogTools,recogMachine)
        {
            Content = new CellContent();
        }

        #region IWrapped


        public override CellWrapper Wrap()
        {
            var adapter = new CheckCellWrapper
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


    }
}