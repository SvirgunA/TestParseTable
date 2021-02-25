using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace TT
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = File.ReadAllText("data.txt");
            
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            
            var rows = doc.DocumentNode.SelectNodes("//tr");
            
            var rowsCount = rows.Count;
            
            var tds = doc.DocumentNode.SelectNodes("//td");

            var colsCounter = 0;
            foreach (var td in tds)
            {
                var colspan = int.TryParse(td.Attributes["colspan"]?.Value, out var i) ? i : 1;
                var rowspan = int.TryParse(td.Attributes["rowspan"]?.Value, out var j) ? j : 1;
                var value = colspan * rowspan;
                colsCounter += value;
            }
            var colsCount = colsCounter / rowsCount;

            var table = new Table();

            var columnId = 0;
            var rowId = 0;
            
            foreach (var row in rows)
            {
                foreach (var td in tds.Where(td => td.ParentNode.StreamPosition == row.StreamPosition))
                {
                    var colspan = td.Attributes["colspan"]?.Value;
                    var rowspan = td.Attributes["rowspan"]?.Value;
                    var width = int.TryParse(colspan, out var i) ? i : 1;
                    var height = int.TryParse(rowspan, out var j) ? j : 1;

                    while (table.IsExists(rowId, columnId ) && columnId <= colsCount)
                    {
                        columnId++;
                    }

                    table.Add(new TableItem
                    {
                        DisplayChar = IdGenerator.GetInstance().GetId(),
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

            Draw(table);
        
            var subTable = table.GetSubTable(0, 0);
            Draw(subTable);
            
            var subTable2 = table.GetSubTable(2, 0);
            Draw(subTable2);
            
            var subTable3 = subTable2.GetSubTable(2, 1);
            Draw(subTable3);
            
            Console.ReadKey();
        }

        private static void Draw(Table table)
        {
            var (cols, rows) = table.CurrentSize;
            foreach (var row in Enumerable.Range(0, rows))
            {
                foreach (var col in Enumerable.Range(0, cols))
                {
                    Console.Write(table.GetId(row, col ) + " ");
                }

                Console.WriteLine("");
            }

            Console.WriteLine("Unique cells: " + table.GetCountOfUniqueCells);
        }
    }
}
