using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace ProjCity2
{
    public sealed class DocumentObjectV2
    {
        public Document document { get; }

        public object docObj;
        private object startEvrethingPage = 0;
        private object tableDefaultBehavior = WdDefaultTableBehavior.wdWord9TableBehavior;
        private object tableAutoFitBehavior = WdAutoFitBehavior.wdAutoFitWindow;

        bool breakPage;

        #region Creating Main Order Document.
        public DocumentObjectV2(Application wordApp, List<string> globalOrdersList,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneSheetsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneForPerimetrsDictionary3D,
            Dictionary<string, List<string>> globalPerimetrsMaterialsList3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalMainCompositionsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBlocksDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBurletsDictionary3D)
        {
            docObj = System.Reflection.Missing.Value;
            document = wordApp.Documents.Add(ref docObj, ref docObj, ref docObj, ref docObj);

            document.Sections[1].Footers[(WdHeaderFooterIndex)1].PageNumbers.Add(1, true);

            breakPage = false;

            foreach (string orderId in globalOrdersList)
            {
                if (globalPolyurethaneSheetsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Листы ППУ", globalPolyurethaneSheetsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalPolyurethaneForPerimetrsDictionary3D.ContainsKey(orderId) & globalPerimetrsMaterialsList3D.ContainsKey(orderId))
                    if (globalPolyurethaneForPerimetrsDictionary3D[orderId].Count != 0 & globalPerimetrsMaterialsList3D[orderId].Count != 0)
                    { CreatePage(orderId, globalPerimetrsMaterialsList3D[orderId], globalPolyurethaneForPerimetrsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalMainCompositionsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Составы", globalMainCompositionsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalBlocksDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Пружинные блоки", globalBlocksDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalUltrCutsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Крой(Ультразвук)", globalUltrCutsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalV16CutsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Крой(V16)", globalV16CutsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalKaterCutsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Крой(Катерман)", globalKaterCutsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalNotStegCutsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Крой(Не стегается)", globalNotStegCutsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalUltrCoversDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Чехлы(Ультразвук)", globalUltrCoversDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalV16CoversDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Чехлы(V16)", globalV16CoversDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalKaterCoversDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Чехлы(Катерман)", globalKaterCoversDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalNotStegCoversDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Чехлы(Не стегается)", globalNotStegCoversDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }
                if (globalBurletsDictionary3D.ContainsKey(orderId))
                { CreatePage(orderId, "Бурлеты", globalBurletsDictionary3D[orderId], breakPage); if (!breakPage) breakPage = true; }

                document.FormattingShowNumbering = true;
            }
        }
        #endregion

        #region Creating Total Order Document.
        public DocumentObjectV2(Application wordApp,
            List<string> globalOrdersList,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneSheetsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneForPerimetrsDictionary3D,
            Dictionary<string, List<string>> globalPerimetrsMaterialsList3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalMainCompositionsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBlocksDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCoversDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBurletsDictionary3D,

            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMattressesDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneSheetsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneForPerimetrsDictionary4D,
            Dictionary<string, Dictionary<string, List<string>>> globalPerimetrsMaterialsList4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMainCompositionsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBlocksDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalUltrCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalV16CutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalKaterCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalNotStegCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalUltrCoversDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalV16CoversDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalKaterCoversDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalNotStegCoversDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBurletsDictioneary4D)
            : this(wordApp, globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D,
                globalPerimetrsMaterialsList3D, globalMainCompositionsDictionary3D, globalBlocksDictionary3D, 
                globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D, globalNotStegCutsDictionary3D,
                globalUltrCoversDictionary3D, globalV16CoversDictionary3D, globalKaterCoversDictionary3D, globalNotStegCoversDictionary3D,
                globalBurletsDictionary3D)
        {
            foreach (var dict in globalMattressesDictionary4D)
                foreach (var item in dict.Value)
                {
                    CreatePage(dict.Key + "\n" + item.Key, "Матрасы", globalMattressesDictionary4D[dict.Key][item.Key], true);
                    if (globalPolyurethaneSheetsDictionary4D.ContainsKey(dict.Key))
                        if (globalPolyurethaneSheetsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Листы ППУ", globalPolyurethaneSheetsDictionary4D[dict.Key][item.Key], true);
                    if (globalPolyurethaneForPerimetrsDictionary4D.ContainsKey(dict.Key) & globalPerimetrsMaterialsList4D.ContainsKey(dict.Key))
                        if (globalPolyurethaneForPerimetrsDictionary4D[dict.Key].ContainsKey(item.Key) & globalPerimetrsMaterialsList4D[dict.Key].ContainsKey(item.Key))
                            if (globalPolyurethaneForPerimetrsDictionary4D[dict.Key][item.Key].Count != 0 & globalPerimetrsMaterialsList4D[dict.Key][item.Key].Count != 0)
                                CreatePage(dict.Key + "\n" + item.Key, globalPerimetrsMaterialsList4D[dict.Key][item.Key], globalPolyurethaneForPerimetrsDictionary4D[dict.Key][item.Key], true);
                    if (globalMainCompositionsDictionary4D.ContainsKey(dict.Key))
                        if (globalMainCompositionsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Составы", globalMainCompositionsDictionary4D[dict.Key][item.Key], true);
                    if (globalBlocksDictionary4D.ContainsKey(dict.Key))
                        if (globalBlocksDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Пружинные блоки", globalBlocksDictionary4D[dict.Key][item.Key], true);
                    if (globalUltrCutsDictionary4D.ContainsKey(dict.Key))
                        if (globalUltrCutsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Крой(Ультразвук)", globalUltrCutsDictionary4D[dict.Key][item.Key], true);
                    if (globalV16CutsDictionary4D.ContainsKey(dict.Key))
                        if (globalV16CutsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Крой(V16)", globalV16CutsDictionary4D[dict.Key][item.Key], true);
                    if (globalKaterCutsDictionary4D.ContainsKey(dict.Key))
                        if (globalKaterCutsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Крой(Катерман)", globalKaterCutsDictionary4D[dict.Key][item.Key], true);
                    if (globalNotStegCutsDictionary4D.ContainsKey(dict.Key))
                        if (globalNotStegCutsDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Крой(Не стегается)", globalNotStegCutsDictionary4D[dict.Key][item.Key], true);
                    if (globalUltrCoversDictionary4D.ContainsKey(dict.Key))
                        if (globalUltrCoversDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Чехлы(Ультразвук)", globalUltrCoversDictionary4D[dict.Key][item.Key], true);
                    if (globalV16CoversDictionary4D.ContainsKey(dict.Key))
                        if (globalV16CoversDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Чехлы(V16)", globalV16CoversDictionary4D[dict.Key][item.Key], true);
                    if (globalKaterCoversDictionary4D.ContainsKey(dict.Key))
                        if (globalKaterCoversDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Чехлы(Катерман)", globalKaterCoversDictionary4D[dict.Key][item.Key], true);
                    if (globalNotStegCoversDictionary4D.ContainsKey(dict.Key))
                        if (globalNotStegCoversDictionary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Чехлы(Не стегается)", globalNotStegCoversDictionary4D[dict.Key][item.Key], true);
                    if (globalBurletsDictioneary4D.ContainsKey(dict.Key))
                        if (globalBurletsDictioneary4D[dict.Key].ContainsKey(item.Key))
                            CreatePage(dict.Key + "\n" + item.Key, "Бурлеты", globalBurletsDictioneary4D[dict.Key][item.Key], true);
                }
        }
        #endregion

        private void CreatePage(string key, string sectionName, Dictionary<string, Dictionary<string, int>> mainDictionary, bool breakPage)
        {
            Range startPageRange = document.Range(ref startEvrethingPage, ref startEvrethingPage);
            startPageRange.Text = $"{key}\n{sectionName}" + Environment.NewLine;
            object endContent = startPageRange.End;

            int rowPos = 2;
            foreach (var dict in mainDictionary)
            {
                Range contentRange = document.Range(ref endContent, ref endContent);
                contentRange.Text = dict.Key;
                object startTable = contentRange.End;

                Range startTableRange = document.Range(ref startTable, ref startTable);
                Table table = document.Tables.Add(startTableRange, dict.Value.Count + 1, 2, tableDefaultBehavior, tableAutoFitBehavior);
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
                Range startNewLineRange = document.Range(ref startNewLine, ref startNewLine);
                startNewLineRange.Text = Environment.NewLine;
            }
            if (breakPage)
            {
                object endPage = document.Tables[mainDictionary.Count].Range.End;
                Range endPageRange = document.Range(ref endPage, ref endPage);
                endPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
        }

        //private void CreatePage(string key, List<string> matherialsList, Dictionary<string, Dictionary<string, int>> mainDictionary, bool breakPage)
        //{
        //    Range startPageRange = document.Range(ref startEvrethingPage, ref startEvrethingPage);
        //    startPageRange.Text = $"{key} : Периметр ППУ";
        //    object tablePosition = startPageRange.End;

        //    Range startTableRange = document.Range(ref tablePosition, ref tablePosition);
        //    Table table = document.Tables.Add(startTableRange, matherialsList.Count + 1, mainDictionary.Count + 1, tableDefaultBehavior, tableAutoFitBehavior);
        //    int rowPos = 2, columnPos = 2;
        //    foreach (var item in mainDictionary)
        //    {
        //        table.Cell(1, columnPos).Range.Text = item.Key;
        //        foreach (var str in matherialsList)
        //        {
        //            table.Cell(rowPos, 1).Range.Text = str;
        //            if (item.Value.ContainsKey(str))
        //                table.Cell(rowPos, columnPos).Range.Text = item.Value[str].ToString();
        //            rowPos++;
        //        }
        //        rowPos = 2;
        //        columnPos++;
        //    }
        //    if (breakPage) 
        //    {
        //        object endPage = table.Range.End;
        //        Range endPageRange = document.Range(ref endPage, ref endPage);
        //        endPageRange.InsertBreak(WdBreakType.wdPageBreak);
        //    } 
        //}

        private void CreatePage(string key, List<string> matherialsList, Dictionary<string, Dictionary<string, int>> mainDictionary, bool breakPage)
        {
            Range startPageRange = document.Range(ref startEvrethingPage, ref startEvrethingPage);
            startPageRange.Text = $"{key} : Периметр ППУ";
            object tablePosition = startPageRange.End;

            Range startTableRange = document.Range(ref tablePosition, ref tablePosition);
            Table table = document.Tables.Add(startTableRange, mainDictionary.Count + 1, matherialsList.Count + 1, tableDefaultBehavior, tableAutoFitBehavior);
            int rowPos = 2, columnPos = 2;
            foreach (var item in mainDictionary)
            {
                table.Cell(rowPos, 1).Range.Text = item.Key;
                foreach (var str in matherialsList)
                {
                    table.Cell(1, columnPos).Range.Text = str;
                    if (item.Value.ContainsKey(str))
                        table.Cell(rowPos, columnPos).Range.Text = item.Value[str].ToString();
                    columnPos++;
                }
                columnPos = 2;
                rowPos++;
            }
            if (breakPage)
            {
                object endPage = table.Range.End;
                Range endPageRange = document.Range(ref endPage, ref endPage);
                endPageRange.InsertBreak(WdBreakType.wdPageBreak);
            }
        }
    }
}
