using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcog.Core.Forms.Cells
{
    public class CellEventArgs : EventArgs
    {
        public Cell Cell { get; }

        public CellEventArgs(Cell cell)
        {
            Cell = cell;
        }
    }
}
