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
        private List<string> globalPerimetrsMaterialsList;

        private Dictionary<string, Dictionary<string, int>> globalMainCompositionsDictionary;

        private Dictionary<string, Dictionary<string, int>> globalUltrCutsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalV16CutsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalKaterCutsDictionary;
        private Dictionary<string, Dictionary<string, int>> globalNotStegCutsDictionary;

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
            if (globalPerimetrsMaterialsList == null)
                globalPerimetrsMaterialsList = new List<string>();
            if (globalMainCompositionsDictionary == null)
                globalMainCompositionsDictionary = new Dictionary<string, Dictionary<string, int>>();
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
                    {
                        globalPolyurethaneForPerimetrsDictionary.Add(dict.Key, dict.Value);

                        foreach (var item in dict.Value)
                            if (!globalPerimetrsMaterialsList.Contains(item.Key))
                                globalPerimetrsMaterialsList.Add(item.Key);
                    }   
                    else
                    {
                        foreach (var item in dict.Value)
                        {
                            if (!globalPerimetrsMaterialsList.Contains(item.Key))
                                globalPerimetrsMaterialsList.Add(item.Key);

                            if (!globalPolyurethaneForPerimetrsDictionary[dict.Key].ContainsKey(item.Key))
                                globalPolyurethaneForPerimetrsDictionary[dict.Key].Add(item.Key, item.Value);
                            else
                                globalPolyurethaneForPerimetrsDictionary[dict.Key][item.Key] += item.Value;
                        }
                    }
                }

                foreach (var dict in obj.dictMainComposition)
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

                if (obj.dictUltrCut != null)
                {
                    globalUltrCutsDictionary = new Dictionary<string, Dictionary<string, int>>();

                    foreach (var dict in obj.dictUltrCut)
                    {
                        if (!globalUltrCutsDictionary.ContainsKey(dict.Key))
                            globalUltrCutsDictionary.Add(dict.Key, dict.Value);
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalUltrCutsDictionary[dict.Key].ContainsKey(item.Key))
                                    globalUltrCutsDictionary[dict.Key].Add(item.Key, item.Value);
                                else
                                    globalUltrCutsDictionary[dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                    //foreach (var item in globalUltrCutsDictionary)
                    //    MessageBox.Show(item.Key,"Ультразвук");
                }

                if (obj.dictV16Cut != null)
                {
                    globalV16CutsDictionary = new Dictionary<string, Dictionary<string, int>>();

                    foreach(var dict in obj.dictV16Cut)
                    {
                        if (!globalV16CutsDictionary.ContainsKey(dict.Key))
                            globalV16CutsDictionary.Add(dict.Key, dict.Value);
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalV16CutsDictionary[dict.Key].ContainsKey(item.Key))
                                    globalV16CutsDictionary[dict.Key].Add(item.Key, item.Value);
                                else
                                    globalV16CutsDictionary[dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                    //foreach (var item in globalV16CutsDictionary)
                    //    MessageBox.Show(item.Key,"V-16");
                }

                if (obj.dictKaterCut != null)
                {
                    globalKaterCutsDictionary = new Dictionary<string, Dictionary<string, int>>();

                    foreach (var dict in obj.dictKaterCut)
                    {
                        if (!globalKaterCutsDictionary.ContainsKey(dict.Key))
                            globalKaterCutsDictionary.Add(dict.Key, dict.Value);
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalKaterCutsDictionary[dict.Key].ContainsKey(item.Key))
                                    globalKaterCutsDictionary[dict.Key].Add(item.Key, item.Value);
                                else
                                    globalKaterCutsDictionary[dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                    //foreach (var item in globalKaterCutsDictionary)
                    //    MessageBox.Show(item.Key,"Катерман");
                }

                if(obj.dictNotStegCut != null)
                {
                    globalNotStegCutsDictionary = new Dictionary<string, Dictionary<string, int>>();

                    foreach (var dict in obj.dictNotStegCut)
                    {
                        if (!globalNotStegCutsDictionary.ContainsKey(dict.Key))
                            globalNotStegCutsDictionary.Add(dict.Key, dict.Value);
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalNotStegCutsDictionary[dict.Key].ContainsKey(item.Key))
                                    globalNotStegCutsDictionary[dict.Key].Add(item.Key, item.Value);
                                else
                                    globalNotStegCutsDictionary[dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                    //foreach (var item in globalNotStegCutsDictionary)
                    //    MessageBox.Show(item.Key,"Не стегается");
                }

                foreach (var dict in obj.dictBurlet)
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

            wordApp.AddDocument(globalPolyurethaneSheetsDictionary, globalPolyurethaneForPerimetrsDictionary, globalPerimetrsMaterialsList, globalMainCompositionsDictionary, globalUltrCutsDictionary, globalV16CutsDictionary, globalKaterCutsDictionary, globalNotStegCutsDictionary, globalBurletsDictionary);

            globalPolyurethaneSheetsDictionary.Clear();
            globalPolyurethaneForPerimetrsDictionary.Clear();
            globalMainCompositionsDictionary.Clear();

            globalUltrCutsDictionary?.Clear();
            globalV16CutsDictionary?.Clear();
            globalKaterCutsDictionary?.Clear();
            globalNotStegCutsDictionary?.Clear();

            globalBurletsDictionary.Clear();

            globalTypesList.Clear();
            listBoxTypesList.Items.Clear();

            txtCustomLenght.Clear();
            txtCustomWidth.Clear();
            txtNumbers.Clear();

            listBoxMattressList.SelectedItem = null;
            cmbSizes.SelectedItem = null;
        }

        #endregion
    }
}
