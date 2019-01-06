using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fcog.Core.Forms.Cells;

namespace Fcog.Core.Serialization.Cells
{
  public  class TextCellWrapper:CellWrapper
    {
        public override Cell UnWrap()
        {
            var result = new TextCell(Guid, RecogTools.UnWrap(), RecogMachine.UnWrap())
            {
                Rectangle = Rectangle,
                Content = Content,
                Index = Index,
                Label = Label,
                DistanceFromMarker = DistanceFromMarker
            };

            return result;

        }
    }
}
