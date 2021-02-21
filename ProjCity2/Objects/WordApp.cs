using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace ProjCity2
{
    public sealed class WordApp
    {
        private Application wordApp;
        private DocumentObjectV2 documentForPrint;

        public WordApp()
        {
            wordApp = new Application();
            wordApp.Visible = true;
        }

        #region Methods.
        public void AddDocument(List<string> globalOrdersList,
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
            documentForPrint = new DocumentObjectV2(wordApp, globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D,
                globalPerimetrsMaterialsList3D, globalMainCompositionsDictionary3D, globalBlocksDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D,
                globalNotStegCutsDictionary3D, globalUltrCoversDictionary3D, globalV16CoversDictionary3D, globalKaterCoversDictionary3D, globalNotStegCoversDictionary3D, globalBurletsDictionary3D);
        }

        public void AddDocument(List<string> globalOrdersList,
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
        {
            documentForPrint = new DocumentObjectV2(wordApp, globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D,
                globalPerimetrsMaterialsList3D, globalMainCompositionsDictionary3D, globalBlocksDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D,
                globalNotStegCutsDictionary3D, globalUltrCoversDictionary3D, globalV16CoversDictionary3D, globalKaterCoversDictionary3D, globalNotStegCoversDictionary3D,
                globalBurletsDictionary3D, globalMattressesDictionary4D, globalPolyurethaneSheetsDictionary4D, globalPolyurethaneForPerimetrsDictionary4D,
                globalPerimetrsMaterialsList4D, globalMainCompositionsDictionary4D, globalBlocksDictionary4D, globalUltrCutsDictionary4D, globalV16CutsDictionary4D, globalKaterCutsDictionary4D,
                globalNotStegCutsDictionary4D, globalUltrCoversDictionary4D, globalV16CoversDictionary4D, globalKaterCoversDictionary4D, globalNotStegCoversDictionary4D, globalBurletsDictioneary4D);
        }

        public void Print()
        {
            object copies = "1";
            object pages = "";
            object range = WdPrintOutRange.wdPrintAllDocument;
            object items = WdPrintOutItem.wdPrintDocumentContent;
            object pageType = WdPrintOutPages.wdPrintAllPages;
            object oTrue = true;
            object oFalse = false;
            object missing = documentForPrint.docObj;

            documentForPrint.document.PrintOut(ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
            ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
            ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);
        }

        public void CloseWord() => wordApp.Quit();
        #endregion
    }
}