using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TT
{
    class Program
    {
        private static string RowsCount = @"<tr";
        private static string TData = @"<td[^>]*?(?:(?:colspan|rowspan)=(?<p>['""]?)(?<colspan>\d*)\k<p>|>)";

        static void Main(string[] args)
        {
            var text = System.IO.File.ReadAllText("data.txt");
            var rowsCount = Regex.Matches(text, RowsCount).Count;
            var cols = Regex.Matches(text, TData);

            var colsCounter = cols.Aggregate(0,
                (current, next) => current + (int.TryParse(next.Groups["colspan"].Value, out var value) ? value : 1));

            var colsCount = colsCounter / rowsCount;
            
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(text);
            var table = doc.DocumentNode.SelectSingleNode("//table");
            var rows = table.SelectNodes("//tr");
            var list = new List<TableItem>();
            var columnId = 0;
            var rowId = 0;
            foreach (var row in rows)
            {
                var tds = row.SelectNodes("//td");
                foreach (var td in tds)
                {
                    if (td.ParentNode.StreamPosition != row.StreamPosition) continue;
                    var colspan = td.Attributes["colspan"]?.Value;
                    var rowspan = td.Attributes["rowspan"]?.Value;
                    var width = int.TryParse(colspan, out var i) ? i : 1;
                    var height = int.TryParse(rowspan, out var j) ? j : 1;

                    while (IsBelongTo(list, columnId, rowId) && columnId <= colsCounter)
                    {
                        columnId++;
                    }
                    
                    list.Add(new TableItem
                    {
                        Id = IdGenerator.GetInstance().GetId(),
                        Col = columnId,
                        Row = rowId,
                        Width = width,
                        Height = height,
                    });
                    columnId += width;
                }

                columnId = 0;
                rowId++;
            }

            Console.ReadKey();
        }

        private static bool IsBelongTo(IEnumerable<TableItem> list, int colId, int rowId)
        {
            var a = list.FirstOrDefault(i =>
                i.Row <= rowId && i.Row + i.Height > rowId && i.Col <= colId && i.Col + i.Width > colId);
            return a != null;
        }
        
        /*cell.row <= 3 && 
cell.row + cell.h >3 && 
cell.col <= 2 && 
cell.col + cell.w > 2*/
    }
}
