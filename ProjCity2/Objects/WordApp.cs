using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace ProjCity2.Objects
{
    public class WordApp
    {
        private Application wordApp;
        private List<DocumentObject> docksList;
        private List<DocumentObjectV2> docksListV2;

        public WordApp()
        {
            wordApp = new Application();
            wordApp.Visible = true;

            docksList = new List<DocumentObject>();
            docksListV2 = new List<DocumentObjectV2>();
        }

        #region Methods.

        public void AddDocument(string orderId ,Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, List<string> globalPerimetrsMaterialsList, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalUltrCutsDictionary, Dictionary<string, Dictionary<string, int>> globalV16CutsDictionary, Dictionary<string, Dictionary<string, int>> globalKaterCutsDictionary, Dictionary<string, Dictionary<string, int>> globalNotStegCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
            docksList.Add(new DocumentObject(wordApp, orderId, globalPolyurethaneSheetsDictionary, globalPolyurethaneForPerimetrsDictionary, globalPerimetrsMaterialsList, globalMainCompositionsDictionary, globalUltrCutsDictionary, globalV16CutsDictionary, globalKaterCutsDictionary, globalNotStegCutsDictionary, globalBurletsDictionary));
        }

        public void AddDocument(List<string> globalOrdersList,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneSheetsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneForPerimetrsDictionary3D,
            Dictionary<string, List<string>> globalPerimetrsMaterialsList3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalMainCompositionsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCutsDictionary3D,
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBurletsDictionary3D,

            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMattressesDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneSheetsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneForPerimetrsDictionary4D,
            Dictionary<string, Dictionary<string, List<string>>> globalPerimetrsMaterialsList4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMainCompositionsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalUltrCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalV16CutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalKaterCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalNotStegCutsDictionary4D,
            Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBurletsDictioneary4D)
        {
            docksListV2.Add(new DocumentObjectV2(wordApp, globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D,
                globalPerimetrsMaterialsList3D, globalMainCompositionsDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D,
                globalNotStegCutsDictionary3D, globalBurletsDictionary3D, globalMattressesDictionary4D, globalPolyurethaneSheetsDictionary4D, globalPolyurethaneForPerimetrsDictionary4D,
                globalPerimetrsMaterialsList4D, globalMainCompositionsDictionary4D, globalUltrCutsDictionary4D, globalV16CutsDictionary4D, globalKaterCutsDictionary4D,
                globalNotStegCutsDictionary4D, globalBurletsDictioneary4D));
        }

        #endregion
    }
}