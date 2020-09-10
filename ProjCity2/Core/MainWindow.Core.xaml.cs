using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EntityModels;
using ProjCity2.Objects;

namespace ProjCity2
{
    public partial class MainWindow
    {
        private WordApp wordApp;

        private List<MattressObject> globalTypesList;

        private Dictionary<string, Dictionary<string, int>> globalPolyurethaneSheetsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalPolyurethaneForPerimetrsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalCutsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalBurletsDictionary;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods.

        private void MattressesListInsert(Mattresses mtrs) => listBoxMattressList.Items.Add(mtrs);

        private void SizesCBInsert(Sizes size) => cmbSizes.Items.Add(size);

        private void SeriesCBInsert(Series series) => cmbSeries.Items.Add(series);

        private void AddMattressObject(Mattresses mattress, Sizes size, string customLenght, string customWidth, string numbers)
        {
            int tempNumbers;
            try
            {
                tempNumbers = Convert.ToInt32(numbers);
            }
            catch
            {
                tempNumbers = 1;
            }

            int? tempLenght = null, tempWidth = null;
            if(size == null)
            {
                try
                {
                    tempLenght = Convert.ToInt32(customLenght);
                    tempWidth = Convert.ToInt32(customWidth);
                }
                catch
                {
                    tempLenght = null;
                    tempWidth = null;
                }
            }

            if (globalTypesList == null)
                globalTypesList = new List<MattressObject>();

            MattressObject tempMattressObject = new MattressObject(mattress, size, tempLenght, tempWidth, tempNumbers);

            if (globalTypesList.Count != 0)
            {
                foreach (MattressObject item in globalTypesList)
                {
                    if (item.Equals(tempMattressObject))
                    {
                        tempNumbers += item.Numbers;
                        globalTypesList.Remove(item);
                        tempMattressObject = new MattressObject(mattress, size, tempLenght, tempWidth, tempNumbers);
                        break;
                    }
                }
            }

            globalTypesList.Add(tempMattressObject);

            listBoxTypesList.Items.Clear();
            foreach (MattressObject obj in globalTypesList)
                listBoxTypesList.Items.Add(obj);

        }

        private void CreateDocument()
        {
            if (globalPolyurethaneSheetsDictionary == null)
                globalPolyurethaneSheetsDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (globalPolyurethaneForPerimetrsDictionary == null)
                globalPolyurethaneForPerimetrsDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (globalMainCompositionsDictionary == null)
                globalMainCompositionsDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (globalCutsDictionary == null)
                globalCutsDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (globalBurletsDictionary == null)
                globalBurletsDictionary = new Dictionary<string, Dictionary<string, int>>();

            foreach(MattressObject obj in globalTypesList)
            {
                foreach(var dict in obj.dictPolyurethaneSheet)
                {
                    if (!globalPolyurethaneSheetsDictionary.ContainsKey(dict.Key))
                        globalPolyurethaneSheetsDictionary.Add(dict.Key, dict.Value);
                    else
                    {
                        foreach(var item in dict.Value)
                        {
                            if (!globalPolyurethaneSheetsDictionary[dict.Key].ContainsKey(item.Key))
                                globalPolyurethaneSheetsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalPolyurethaneSheetsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }

                foreach (var dict in obj.dictPolyurethaneForPerimetr)
                {
                    if (!globalPolyurethaneForPerimetrsDictionary.ContainsKey(dict.Key))
                        globalPolyurethaneForPerimetrsDictionary.Add(dict.Key, dict.Value);
                    else
                    {
                        foreach (var item in dict.Value)
                        {
                            if (!globalPolyurethaneForPerimetrsDictionary[dict.Key].ContainsKey(item.Key))
                                globalPolyurethaneForPerimetrsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalPolyurethaneForPerimetrsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }

                foreach (var dict in obj.dictPolyurethaneForPerimetr)
                {
                    if (!globalMainCompositionsDictionary.ContainsKey(dict.Key))
                        globalMainCompositionsDictionary.Add(dict.Key, dict.Value);
                    else
                    {
                        foreach (var item in dict.Value)
                        {
                            if (!globalMainCompositionsDictionary[dict.Key].ContainsKey(item.Key))
                                globalMainCompositionsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalMainCompositionsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }

                foreach (var dict in obj.dictPolyurethaneForPerimetr)
                {
                    if (!globalCutsDictionary.ContainsKey(dict.Key))
                        globalCutsDictionary.Add(dict.Key, dict.Value);
                    else
                    {
                        foreach (var item in dict.Value)
                        {
                            if (!globalCutsDictionary[dict.Key].ContainsKey(item.Key))
                                globalCutsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalCutsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }

                foreach (var dict in obj.dictPolyurethaneForPerimetr)
                {
                    if (!globalBurletsDictionary.ContainsKey(dict.Key))
                        globalBurletsDictionary.Add(dict.Key, dict.Value);
                    else
                    {
                        foreach (var item in dict.Value)
                        {
                            if (!globalBurletsDictionary[dict.Key].ContainsKey(item.Key))
                                globalBurletsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalBurletsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }
            }

            wordApp = new WordApp();

            wordApp.AddDocument(globalPolyurethaneSheetsDictionary, globalPolyurethaneForPerimetrsDictionary, globalMainCompositionsDictionary, globalCutsDictionary, globalBurletsDictionary);
        }

        #endregion
    }
}
