using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using Fcog.Core.Forms.Cells;

namespace Fcog.Controls.Wpf.Forms.Cells
{
    public static class CellControlsDictionary
    {
        private static readonly Dictionary<Type, Func<Cell,  Canvas,  DeleteCellHandler, ICellControl>> cellControlsDictionary =
            new Dictionary<Type, Func<Cell, Canvas,DeleteCellHandler, ICellControl>>
            {
                {typeof(CheckCell), GetCheckCellControl},
                {typeof(LetterCell), GetLetterCellControl},
                {typeof(TextCell), GetTextCellControl},
                {typeof(RadioCell), GetRadioCellControl},
            };



        private static ICellControl GetRadioCellControl(Cell cell,  Canvas canvas,  DeleteCellHandler cellDeleteAction)
        {
            return new RadioCellControl { Cell = cell, ImageCanvas = canvas,  DeleteCellAction = cellDeleteAction };
        }

        private static ICellControl GetCheckCellControl(Cell cell,  Canvas canvas,  DeleteCellHandler cellDeleteAction)
        {
            return new CheckCellControl { Cell = cell,  ImageCanvas = canvas,  DeleteCellAction = cellDeleteAction };
        }

        private static ICellControl GetTextCellControl(Cell cell, Canvas canvas,  DeleteCellHandler cellDeleteAction)
        {
            return new TextCellControl { Cell = cell,  ImageCanvas = canvas,  DeleteCellAction = cellDeleteAction };
        }

        private static ICellControl GetLetterCellControl(Cell cell,  Canvas canvas,  DeleteCellHandler cellDeleteAction)
        {
            return new LetterCellControl { Cell = cell,  ImageCanvas = canvas,  DeleteCellAction = cellDeleteAction };
        }

        private static ICellControl GetUnknownCellControl(Cell cell, Canvas canvas, DeleteCellHandler cellDeleteAction)
        {
            return new UnknownCellControl { Cell = cell,  ImageCanvas = canvas,  DeleteCellAction = cellDeleteAction };
        }


        public static ICellControl GetCellControl(Cell cell,  Canvas canvas, DeleteCellHandler cellDeleteAction)
        {
            Func<Cell,  Canvas,  DeleteCellHandler, ICellControl> getCellFunction;
            cellControlsDictionary.TryGetValue(cell.GetType(), out getCellFunction);

            return getCellFunction != null ? getCellFunction(cell,  canvas,  cellDeleteAction) : GetUnknownCellControl(cell,  canvas,  cellDeleteAction);
        }
    }
}