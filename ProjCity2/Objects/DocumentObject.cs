using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace ProjCity2
{
    public class DocumentObject
    {
        public Document document { get; }

        public DocumentObject(Application wordApp, Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, List<string> globalPerimetrsMaterialsList, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
            object docObj = System.Reflection.Missing.Value;
            document = wordApp.Documents.Add(ref docObj, ref docObj, ref docObj, ref docObj);

            object startEverythingPages = 0;

            Range startPolyurethaneSheetsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);

            object endContent = startPolyurethaneSheetsPageRange.End;

            foreach (var dict in globalPolyurethaneSheetsDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;

                object startNewLine = contentRange.End;
                Range newLineRange = document.Range(ref startNewLine, ref startNewLine);
                //newLineRange.Text = Environment.NewLine;

                object startTable = newLineRange.End;
                object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, ref tableDefaultBehavior, ref tableAutoFitBehavior);

                table.Cell(1, 1).Range.Text = "Размер";
                table.Cell(1, 2).Range.Text = "Количество";

                int rowPos = 2;
                foreach (var item in dict.Value)
                {
                    table.Cell(rowPos, 1).Range.Text = item.Key;
                    table.Cell(rowPos, 2).Range.Text = item.Value.ToString();
                    rowPos++;
                }
                rowPos = 2;

                object startNewLine1 = table.Range.End;

                Range newLineRange1 = document.Range(ref startNewLine1, ref startNewLine1);
                newLineRange1.Text = Environment.NewLine /*+ Environment.NewLine*/;
            }

            if(globalPolyurethaneForPerimetrsDictionary!=null && globalPerimetrsMaterialsList!=null)
            {
                Range startPolyurethaneForPerimetrsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);

                object perimetrsTablePosition = startPolyurethaneForPerimetrsPageRange.End;
                object perimetrsTableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object perimetrsTableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range perimetrsTableRange = document.Range(ref perimetrsTablePosition, ref perimetrsTablePosition);
                Table perimetrsTable = document.Tables.Add(perimetrsTableRange, globalPerimetrsMaterialsList.Count + 1, globalPolyurethaneForPerimetrsDictionary.Count + 1, ref perimetrsTableDefaultBehavior, ref perimetrsTableAutoFitBehavior);

                int rowPosition = 2, columnPosition = 2;
                foreach (var item in globalPolyurethaneForPerimetrsDictionary)
                {
                    perimetrsTable.Cell(1, columnPosition).Range.Text = item.Key;
                    foreach (string str in globalPerimetrsMaterialsList)
                    {
                        perimetrsTable.Cell(rowPosition, 1).Range.Text = str;

                        if (item.Value.ContainsKey(str))
                            perimetrsTable.Cell(rowPosition, columnPosition).Range.Text = item.Value[str].ToString();
                        rowPosition++;
                    }
                    rowPosition = 2;
                    columnPosition++;
                }

                object endPolyurethaneForPerimetrsPage = perimetrsTable.Range.End;

                Range endPolyurethaneForPerimetrsPageRange = document.Range(ref endPolyurethaneForPerimetrsPage, ref endPolyurethaneForPerimetrsPage);
                endPolyurethaneForPerimetrsPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
        }
            
    }
}
