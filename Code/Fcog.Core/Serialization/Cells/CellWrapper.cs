using System;
using System.Drawing;
using System.Runtime.Serialization;
using Fcog.Core.Forms.Cells;
using Fcog.Core.Forms.Cells.Content;
using Fcog.Core.Recognition;
using Fcog.Core.Serialization.Recognition;

namespace Fcog.Core.Serialization.Cells
{
    [DataContract]
    [KnownType(typeof(CheckCellWrapper)),
     KnownType(typeof(LetterCellWrapper)),
     KnownType(typeof(RadioCellWrapper)),
     KnownType(typeof(TextCellWrapper)),]
    public abstract class CellWrapper:IUnWrapped<Cell>
    {
      [DataMember]
       public RecogMachineWrapper RecogMachine { get; set; }

        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public CellContent Content { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public MarkerDistance DistanceFromMarker { get; set; }

        [DataMember]
        public Rectangle Rectangle { get; set; }

        [DataMember]
        public RecogToolsWrapper RecogTools { get; set; }

        public abstract Cell UnWrap();

    }
}