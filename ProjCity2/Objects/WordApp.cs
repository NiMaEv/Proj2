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

        public void AddDocument(Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary, Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary, Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary, Dictionary<string, Dictionary<string, int>> globalCutsDictionary, Dictionary<string, Dictionary<string, int>> globalBurletsDictionary)
        {
           
        }

        #endregion
    }
}