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

        public WordApp()
        {
            wordApp = new Application();
            wordApp.Visible = true;

            docksList = new List<DocumentObject>();
        }

        #region Methods.

        public void AddDocument(string orderId ,Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, List<string> globalPerimetrsMaterialsList, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalUltrCutsDictionary, Dictionary<string, Dictionary<string, int>> globalV16CutsDictionary, Dictionary<string, Dictionary<string, int>> globalKaterCutsDictionary, Dictionary<string, Dictionary<string, int>> globalNotStegCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
            docksList.Add(new DocumentObject(wordApp, orderId, globalPolyurethaneSheetsDictionary, globalPolyurethaneForPerimetrsDictionary, globalPerimetrsMaterialsList, globalMainCompositionsDictionary, globalUltrCutsDictionary, globalV16CutsDictionary, globalKaterCutsDictionary, globalNotStegCutsDictionary, globalBurletsDictionary));
        }

        #endregion
    }
}