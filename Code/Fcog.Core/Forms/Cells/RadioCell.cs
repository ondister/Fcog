using System;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization.Cells;

namespace Fcog.Core.Forms.Cells
{
    public class RadioCell : Cell
    {
        internal RadioCell(RecogTools recogTools, RecogMachine recogMachine) : base(recogTools, recogMachine)
        {
            Content = new CellContent();
        }

        internal RadioCell(Guid guid, RecogTools recogTools, RecogMachine recogMachine) : base(guid, recogTools,
            recogMachine)
        {
            Content = new CellContent();
        }

        public string GroupName { get; set; }

        #region IWrapped

        public override CellWrapper Wrap()
        {
            var adapter = new RadioCellWrapper
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