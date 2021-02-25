using System;
using System.Collections.Generic;
using System.Linq;

namespace TT
{
    public class Table
    {
        private List<TableItem> Items { get; }

        public Table()
        {
            Items = new List<TableItem>();
        }

        public Table(IEnumerable<TableItem> list)
        {
            Items = list.ToList();
        }

        public (int Cols, int Rows) CurrentSize
        {
            get
            {
                var rows = Items.Select(i => i.Row).Distinct().Count();
                var columns = Items.Aggregate(0, (cur, next) => cur + next.Height * next.Width);
                var cols = columns / rows;
                return (cols, rows);
            }
        }

        public Table GetSubTable(int rowId, int colId)
        {
            var cell = this[rowId, colId];
            if (cell == null) return null;
            var rowOffset = cell.Row;
            var collOffset = cell.Col;
            if (cell.Width > 1)
            {
                var startRow = rowId;
                var find = false;
                while (!find && startRow < CurrentSize.Rows - 1)
                {
                    startRow++;
                    var width = Items.Where(i => i.Row == startRow).Max(i => i.Width);
                    find = width == cell.Width;
                }

                if (startRow == CurrentSize.Rows - 1) startRow++;

                var subList = GetCellsRange(rowId, colId, CurrentSize.Cols, startRow - rowId).ToList();
                foreach (var item in subList)
                {
                    item.Row -= rowOffset;
                    item.Col -= collOffset;
                }

                return new Table(subList);
            }

            return this;
        }


        public void Add(TableItem item) => Items.Add(item);

        public char GetId(int rowId, int colId) =>
            this[rowId, colId]?.DisplayChar ?? '?';

        public bool IsExists(int rowId, int colId) =>
            this[rowId, colId] != null;

        public int GetCountOfUniqueCells =>
            Items.Select(i => i.DisplayChar).Distinct().Count();

        private IEnumerable<TableItem> GetCellsRange(int rowId, int colId, int width, int height)
        {
            return Items.Where(i =>
                i.Col >= colId && i.Col <= colId + width && i.Row >= rowId && i.Row < rowId + height);
        }

        private TableItem this[int rowId, int colId] =>
            Items.FirstOrDefault(i => SearchExpression(i, rowId, colId));

        private static Func<TableItem, int, int, bool> SearchExpression => (i, rowId, colId) =>
            i.Row <= rowId && i.Row + i.Height > rowId && i.Col <= colId && i.Col + i.Width > colId;
    }
}
