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

        public DocumentObject(Microsoft.Office.Interop.Word.Application wordApp, Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, List<string> globalPerimetrsMaterialsList, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalUltrCutsDictionary, Dictionary<string, Dictionary<string, int>> globalV16CutsDictionary, Dictionary<string, Dictionary<string, int>> globalKaterCutsDictionary, Dictionary<string, Dictionary<string, int>> globalNotStegCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
            object docObj = System.Reflection.Missing.Value;
            document = wordApp.Documents.Add(ref docObj, ref docObj, ref docObj, ref docObj);

            object startEverythingPages = 0;

            #region Polyurethane Sheets Page.
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
            #endregion

            #region Polyurethane For Perimtrs Page.
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
            #endregion

            #region Main Composition Page.
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
            #endregion

            #region Cuts Pages.

            #region UltrCuts Page.
            if (globalUltrCutsDictionary != null && globalUltrCutsDictionary.Count != 0)
            {
                Range startUltrCutsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
                endContent = startUltrCutsPageRange.End;

                rowPos = 2;
                foreach (var dict in globalUltrCutsDictionary)
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

                object endUltrCutsPage = document.Tables[globalUltrCutsDictionary.Count].Range.End;
                Range endUltrCutsPageRange = document.Range(ref endUltrCutsPage, ref endUltrCutsPage);
                endUltrCutsPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
            #endregion

            #region V16Cuts Page.
            if (globalV16CutsDictionary != null && globalV16CutsDictionary.Count != 0)
            {
                Range startV16CutsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
                endContent = startV16CutsPageRange.End;

                rowPos = 2;
                foreach (var dict in globalV16CutsDictionary)
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

                object endV16CutsPage = document.Tables[globalV16CutsDictionary.Count].Range.End;
                Range endV16CutsPageRange = document.Range(ref endV16CutsPage, ref endV16CutsPage);
                endV16CutsPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
            #endregion

            #region KaterCuts Page.
            if (globalKaterCutsDictionary != null && globalKaterCutsDictionary.Count != 0)
            {
                Range startKaterCutsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
                endContent = startKaterCutsPageRange.End;

                rowPos = 2;
                foreach (var dict in globalKaterCutsDictionary)
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

                object endKaterCutsPage = document.Tables[globalKaterCutsDictionary.Count].Range.End;
                Range endKaterCutsPageRange = document.Range(ref endKaterCutsPage, ref endKaterCutsPage);
                endKaterCutsPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
            #endregion

            #region NotStegCuts Page.
            if (globalNotStegCutsDictionary != null && globalNotStegCutsDictionary.Count != 0)
            {
                Range startNotStegCutsPageRange = document.Range(ref startEverythingPages, ref startEverythingPages);
                endContent = startNotStegCutsPageRange.End;

                rowPos = 2;
                foreach (var dict in globalUltrCutsDictionary)
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

                object endNotStegCutsPage = document.Tables[globalNotStegCutsDictionary.Count].Range.End;
                Range endNotStegCutsPageRange = document.Range(ref endNotStegCutsPage, ref endNotStegCutsPage);
                endNotStegCutsPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
            #endregion

            #endregion

            #region Burlets Page.
            if (globalBurletsDictionary.Count != 0) 
            {
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
            #endregion
        }

    }
}
