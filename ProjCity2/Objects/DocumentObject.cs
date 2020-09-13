using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Office.Interop.Word;

namespace ProjCity2
{
    public class DocumentObject
    {
        public Document document { get; }

        public DocumentObject(Microsoft.Office.Interop.Word.Application wordApp, Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, List<string> globalPerimetrsMaterialsList, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
            object docObj = System.Reflection.Missing.Value;
            document = wordApp.Documents.Add(ref docObj, ref docObj, ref docObj, ref docObj);

            object startEverythingPages = 0;

            Range startPolyurethaneSheetsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
            object endContent = startPolyurethaneSheetsPageRange.End;

            int rowPos = 2;
            foreach (var dict in globalPolyurethaneSheetsDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;

                object startTable = contentRange.End;
                object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, ref tableDefaultBehavior, ref tableAutoFitBehavior);
                table.Cell(1, 1).Range.Text = "Размер";
                table.Cell(1, 2).Range.Text = "Количество";
                
                foreach (var item in dict.Value)
                {
                    table.Cell(rowPos, 1).Range.Text = item.Key;
                    table.Cell(rowPos, 2).Range.Text = item.Value.ToString();
                    rowPos++;
                }
                rowPos = 2;

                object startNewLine = table.Range.End;
                Range newLineRange = document.Range(ref startNewLine, ref startNewLine);
                newLineRange.Text = Environment.NewLine;
            }

            if (globalPolyurethaneForPerimetrsDictionary.Count != 0 && globalPerimetrsMaterialsList.Count != 0)
            {
                Range startPolyurethaneForPerimetrsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
                startPolyurethaneForPerimetrsPageRange.Text = "Раскрой: ППУ - периметр.";

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

            //Main compositions.
            Range startMainCompositionsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
            endContent = startMainCompositionsPageRange.End;

            rowPos = 2;
            foreach (var dict in globalMainCompositionsDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;

                object startTable = contentRange.End;
                object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, ref tableDefaultBehavior, ref tableAutoFitBehavior);

                table.Cell(1, 1).Range.Text = "Размер";
                table.Cell(1, 2).Range.Text = "Количество";
                
                foreach (var item in dict.Value)
                {
                    table.Cell(rowPos, 1).Range.Text = item.Key;
                    table.Cell(rowPos, 2).Range.Text = item.Value.ToString();
                    rowPos++;
                }
                rowPos = 2;

                object startNewLine = table.Range.End;
                Range newLineRange = document.Range(ref startNewLine, ref startNewLine);
                newLineRange.Text = Environment.NewLine;
            }

            object endMainCompositionPage = document.Tables[globalMainCompositionsDictionary.Count].Range.End;
            Range endMainCompositionPageRange = document.Range(ref endMainCompositionPage, ref endMainCompositionPage);
            endMainCompositionPageRange.InsertBreak(WdBreakType.wdPageBreak);

            //Cuts.
            Range startCutsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
            endContent = startCutsPageRange.End;

            rowPos = 2;
            foreach (var dict in globalCutsDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;

                object startTable = contentRange.End;
                object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, ref tableDefaultBehavior, ref tableAutoFitBehavior);
                table.Cell(1, 1).Range.Text = "Размер";
                table.Cell(1, 2).Range.Text = "Количество";

                foreach (var item in dict.Value)
                {
                    table.Cell(rowPos, 1).Range.Text = item.Key;
                    table.Cell(rowPos, 2).Range.Text = item.Value.ToString();
                    rowPos++;
                }
                rowPos = 2;

                object startNewLine = table.Range.End;
                Range newLineRange = document.Range(ref startNewLine, ref startNewLine);
                newLineRange.Text = Environment.NewLine;
            }

            object endCutsPage = document.Tables[globalCutsDictionary.Count].Range.End;
            Range endCutsPageRange = document.Range(ref endCutsPage, ref endCutsPage);
            endCutsPageRange.InsertBreak(WdBreakType.wdPageBreak);

            //Burlets.
            Range startBurletsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
            endContent = startBurletsPageRange.End;

            rowPos = 2;
            foreach (var dict in globalBurletsDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;

                object startTable = contentRange.End;
                object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
                object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, ref tableDefaultBehavior, ref tableAutoFitBehavior);
                table.Cell(1, 1).Range.Text = "Размер";
                table.Cell(1, 2).Range.Text = "Количество";

                foreach (var item in dict.Value)
                {
                    table.Cell(rowPos, 1).Range.Text = item.Key;
                    table.Cell(rowPos, 2).Range.Text = item.Value.ToString();
                    rowPos++;
                }
                rowPos = 2;

                object startNewLine = table.Range.End;
                Range newLineRange = document.Range(ref startNewLine, ref startNewLine);
                newLineRange.Text = Environment.NewLine;

            }

            object endBurletsPage = document.Tables[globalBurletsDictionary.Count].Range.End;
            Range endBurletsPageRange = document.Range(ref endBurletsPage, ref endBurletsPage);
            endBurletsPageRange.InsertBreak(WdBreakType.wdPageBreak);
        }
         
    }
}
