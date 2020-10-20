using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using EntityModels;
using ProjCity2.Objects;

namespace ProjCity2
{
    public partial class MainWindow
    {
        private WordApp wordApp;

        private List<MattressObject> globalTypesList;

        #region Fields of Main Order: 3D-Dictionaries (Order Id str/( Composition str/( Size str/ Numbers)))
        private List<string> globalOrdersList;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneSheetsDictionary3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneForPerimetrsDictionary3D;
        private Dictionary<string, List<string>> globalPerimetrsMaterialsList3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalMainCompositionsDictionary3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCutsDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CutsDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCutsDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCutsDictionary3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBurletsDictionary3D;
        #endregion
        #region Fields of Orders for Tables: 4D-Dictionaries (Order Id str/( Name of Table str/( Composition str/( Size str, Numbers))))
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMattressesDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneSheetsDictionary4D;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalPolyurethaneForPerimetrsDictionary4D;
        private Dictionary<string, Dictionary<string, List<string>>> globalPerimetrsMaterialsList4D;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalMainCompositionsDictionary4D;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalUltrCutsDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalV16CutsDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalKaterCutsDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalNotStegCutsDictionary4D;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBurletsDictionary4D;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods.

        private void MattressesListInsert(Mattresses mtrs) => listBoxMattressList.Items.Add(mtrs);

        private void SizesCBInsert(Sizes size) => cmbSizes.Items.Add(size);

        private void SeriesCBInsert(Series serie) => cmbSeries.Items.Add(serie);

        private void TablesCBInsert(Tables table) => cmbTables.Items.Add(table);

        private void AddMattressObject()
        {
            int tempNumbers = 1;
            if (txtNumbers.Text.Length != 0)
                tempNumbers = Convert.ToInt32(txtNumbers.Text);
            if (tempNumbers == 0)
                throw new Exception();

            int? tempLenght = null, tempWidth = null;
            if (txtCustomLenght.Text.Length != 0 & txtCustomWidth.Text.Length != 0)
            {
                tempLenght = Convert.ToInt32(txtCustomLenght.Text);
                tempWidth = Convert.ToInt32(txtCustomWidth.Text);
            }

            if (globalTypesList == null)
                globalTypesList = new List<MattressObject>();

            MattressObject tempMattressObject = new MattressObject((Mattresses)listBoxMattressList.SelectedItem, (Sizes)cmbSizes.SelectedItem, tempLenght, tempWidth, tempNumbers, txtOrderId.Text+" : " + txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem);

            if (globalTypesList.Count != 0)
            {
                foreach (MattressObject item in globalTypesList)
                {
                    if (item.Equals(tempMattressObject))
                    {
                        tempNumbers += item.Numbers;
                        globalTypesList.Remove(item);
                        tempMattressObject = new MattressObject((Mattresses)listBoxMattressList.SelectedItem, (Sizes)cmbSizes.SelectedItem, tempLenght, tempWidth, tempNumbers, txtOrderId.Text + " : " + txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem);
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
            #region Initialized Dictionaries.
            if (globalOrdersList == null)
                globalOrdersList = new List<string>();

            if (globalMattressesDictionary4D == null)
                globalMattressesDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalPolyurethaneSheetsDictionary3D == null)
                globalPolyurethaneSheetsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalPolyurethaneSheetsDictionary4D == null)
                globalPolyurethaneSheetsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalPolyurethaneForPerimetrsDictionary3D == null)
                globalPolyurethaneForPerimetrsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalPolyurethaneForPerimetrsDictionary4D == null)
                globalPolyurethaneForPerimetrsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalPerimetrsMaterialsList3D == null)
                globalPerimetrsMaterialsList3D = new Dictionary<string, List<string>>();
            if (globalPerimetrsMaterialsList4D == null)
                globalPerimetrsMaterialsList4D = new Dictionary<string, Dictionary<string, List<string>>>();

            if (globalMainCompositionsDictionary3D == null)
                globalMainCompositionsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalMainCompositionsDictionary4D == null)
                globalMainCompositionsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalUltrCutsDictionary3D == null)
                globalUltrCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalUltrCutsDictionary4D == null)
                globalUltrCutsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalV16CutsDictionary3D == null)
                globalV16CutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalV16CutsDictionary4D == null)
                globalV16CutsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalKaterCutsDictionary3D == null)
                globalKaterCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalKaterCutsDictionary4D == null)
                globalKaterCutsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalNotStegCutsDictionary3D == null)
                globalNotStegCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalNotStegCutsDictionary4D == null)
                globalNotStegCutsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalBurletsDictionary3D == null)
                globalBurletsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalBurletsDictionary4D == null)
                globalBurletsDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();
            #endregion

            foreach(MattressObject obj in globalTypesList)
            {

                if (!globalOrdersList.Contains(obj.OrderInfo))
                    globalOrdersList.Add(obj.OrderInfo);

                if (!globalMattressesDictionary4D.ContainsKey(obj.OrderInfo))
                    globalMattressesDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, new Dictionary<string, Dictionary<string, int>> { { obj.Name, new Dictionary<string, int> { { obj.Size, obj.Numbers } } } } } });
                else
                {
                    if (!globalMattressesDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalMattressesDictionary4D[obj.OrderInfo].Add(obj.TableName, new Dictionary<string, Dictionary<string, int>> { { obj.Name, new Dictionary<string, int> { { obj.Size, obj.Numbers } } } });
                    else
                    {
                        if (!globalMattressesDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(obj.Name))
                            globalMattressesDictionary4D[obj.OrderInfo][obj.TableName].Add(obj.Name, new Dictionary<string, int> { { obj.Size, obj.Numbers } });
                        else
                        {
                            if (!globalMattressesDictionary4D[obj.OrderInfo][obj.TableName][obj.Name].ContainsKey(obj.Size))
                                globalMattressesDictionary4D[obj.OrderInfo][obj.TableName][obj.Name].Add(obj.Size, obj.Numbers);
                        }
                    }
                }

                #region globalPolyuetaneSheetsDictionaries.
                if (!globalPolyurethaneSheetsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalPolyurethaneSheetsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictPolyurethaneSheet));
                else
                {
                    foreach (var dict in obj.dictPolyurethaneSheet)
                    {
                        if (!globalPolyurethaneSheetsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalPolyurethaneSheetsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalPolyurethaneSheetsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                    globalPolyurethaneSheetsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                else
                                    globalPolyurethaneSheetsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                }

                if (!globalPolyurethaneSheetsDictionary4D.ContainsKey(obj.OrderInfo))
                    globalPolyurethaneSheetsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictPolyurethaneSheet) } });
                else
                {
                    if (!globalPolyurethaneSheetsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalPolyurethaneSheetsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictPolyurethaneSheet));
                    else
                    {
                        foreach (var dict in obj.dictPolyurethaneSheet)
                        {
                            if (!globalPolyurethaneSheetsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                globalPolyurethaneSheetsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
                                {
                                    if (!globalPolyurethaneSheetsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                        globalPolyurethaneSheetsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalPolyurethaneSheetsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region globalPolyurethaneForPerimetrsDictionaries.
                if (obj.dictPolyurethaneForPerimetr != null)
                {
                    if (!globalPolyurethaneForPerimetrsDictionary3D.ContainsKey(obj.OrderInfo))
                    {
                        globalPolyurethaneForPerimetrsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictPolyurethaneForPerimetr));

                        globalPerimetrsMaterialsList3D.Add(obj.OrderInfo, new List<string> { });
                        foreach (var dict in obj.dictPolyurethaneForPerimetr)
                            foreach (var item in dict.Value)
                                if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                    globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);
                    }   
                    else
                    {
                        foreach(var dict in obj.dictPolyurethaneForPerimetr)
                        {
                            if(!globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            {
                                globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));

                                foreach (var item in dict.Value)
                                    if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                        globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);
                            }
                            else
                            {
                                foreach(var item in dict.Value)
                                {
                                    if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                        globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);

                                    if (!globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                        globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }

                    if (!globalPolyurethaneForPerimetrsDictionary4D.ContainsKey(obj.OrderInfo))
                    {
                        globalPolyurethaneForPerimetrsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictPolyurethaneForPerimetr) } });

                        globalPerimetrsMaterialsList4D.Add(obj.OrderInfo, new Dictionary<string, List<string>> { { obj.TableName, new List<string> { } } });
                        foreach (var dict in obj.dictPolyurethaneForPerimetr)
                            foreach (var item in dict.Value)
                                if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                    globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);
                    }
                    else
                    {
                        if (!globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        {
                            globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictPolyurethaneForPerimetr));

                            globalPerimetrsMaterialsList4D[obj.OrderInfo].Add(obj.TableName, new List<string> { });
                            foreach (var dict in obj.dictPolyurethaneForPerimetr)
                                foreach (var item in dict.Value)
                                    if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                        globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);
                        }
                        else
                        {
                            foreach (var dict in obj.dictPolyurethaneForPerimetr)
                            {
                                if (!globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                {
                                    globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));

                                    foreach (var item in dict.Value)
                                        if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                            globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);
                                }
                                else
                                {
                                    foreach (var item in dict.Value)
                                    {
                                        if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                            globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);

                                        if (!globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                            globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                        else
                                            globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region globalMainCompositionsDictionaries.
                if (!globalMainCompositionsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalMainCompositionsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictMainComposition));
                else
                {
                    foreach(var dict in obj.dictMainComposition)
                    {
                        if (!globalMainCompositionsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalMainCompositionsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach(var item in dict.Value)
                            {
                                if (!globalMainCompositionsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                    globalMainCompositionsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                else
                                    globalMainCompositionsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                }

                if (!globalMainCompositionsDictionary4D.ContainsKey(obj.OrderInfo))
                    globalMainCompositionsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictMainComposition) } });
                else
                {
                    if (!globalMainCompositionsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalMainCompositionsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictMainComposition));
                    else
                    {
                        foreach (var dict in obj.dictMainComposition)
                        {
                            if (!globalMainCompositionsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                globalMainCompositionsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
                                {
                                    if (!globalMainCompositionsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                        globalMainCompositionsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalMainCompositionsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Cuts Dictionaries.

                #region globalUltrCutsDictionary4D.
                if (obj.dictUltrCut != null) 
                {
                    if (!globalUltrCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalUltrCutsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictUltrCut));
                    else
                    {
                        foreach(var dict in obj.dictUltrCut)
                        {
                            if (!globalUltrCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalUltrCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach(var item in dict.Value)
                                {
                                    if (!globalUltrCutsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                        globalUltrCutsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalUltrCutsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }

                    if (!globalUltrCutsDictionary4D.ContainsKey(obj.OrderInfo))
                        globalUltrCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictUltrCut) } });
                    else
                    {
                        if (!globalUltrCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalUltrCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictUltrCut));
                        else
                        {
                            foreach (var dict in obj.dictUltrCut)
                            {
                                if (!globalUltrCutsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                    globalUltrCutsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                                else
                                {
                                    foreach (var item in dict.Value)
                                    {
                                        if (!globalUltrCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                            globalUltrCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                        else
                                            globalUltrCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region globalV16CutsDictionary4D.
                if (obj.dictV16Cut != null) 
                {
                    if (!globalV16CutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalV16CutsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictV16Cut));
                    else
                    {
                        foreach(var dict in obj.dictV16Cut)
                        {
                            if (!globalV16CutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalV16CutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach(var item in dict.Value)
                                {
                                    if (!globalV16CutsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                        globalV16CutsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalV16CutsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }

                    if (!globalV16CutsDictionary4D.ContainsKey(obj.OrderInfo))
                        globalV16CutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictV16Cut) } });
                    else
                    {
                        if (!globalV16CutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalV16CutsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictV16Cut));
                        else
                        {
                            foreach (var dict in obj.dictV16Cut)
                            {
                                if (!globalV16CutsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                    globalV16CutsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                                else
                                {
                                    foreach (var item in dict.Value)
                                    {
                                        if (!globalV16CutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                            globalV16CutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                        else
                                            globalV16CutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region globalKaterCutsDictionary4D.
                if (obj.dictKaterCut != null)
                {
                    if (!globalKaterCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalKaterCutsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictKaterCut));
                    else
                    {
                        foreach(var dict in obj.dictKaterCut)
                        {
                            if (!globalKaterCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalKaterCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach(var item in dict.Value)
                                {
                                    if (!globalKaterCutsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                        globalKaterCutsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalKaterCutsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }

                    if (!globalKaterCutsDictionary4D.ContainsKey(obj.OrderInfo))
                        globalKaterCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictKaterCut) } });
                    else
                    {
                        if (!globalKaterCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalKaterCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictKaterCut));
                        else
                        {
                            foreach (var dict in obj.dictKaterCut)
                            {
                                if (!globalKaterCutsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                    globalKaterCutsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                                else
                                {
                                    foreach (var item in dict.Value)
                                    {
                                        if (!globalKaterCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                            globalKaterCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                        else
                                            globalKaterCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region globalNotStegCutsDictionary4D.
                if (obj.dictNotStegCut != null)
                {
                    if (!globalNotStegCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalNotStegCutsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictNotStegCut));
                    else
                    {
                        foreach(var dict in obj.dictNotStegCut)
                        {
                            if (!globalNotStegCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalNotStegCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach(var item in dict.Value)
                                {
                                    if (!globalNotStegCutsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                        globalNotStegCutsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalNotStegCutsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }

                    if (!globalNotStegCutsDictionary4D.ContainsKey(obj.OrderInfo))
                        globalNotStegCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictNotStegCut) } });
                    else
                    {
                        if (!globalNotStegCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalNotStegCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictNotStegCut));
                        else
                        {
                            foreach (var dict in obj.dictNotStegCut)
                            {
                                if (!globalNotStegCutsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                    globalNotStegCutsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                                else
                                {
                                    foreach (var item in dict.Value)
                                    {
                                        if (!globalNotStegCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                            globalNotStegCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                        else
                                            globalNotStegCutsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region globalBurletsDitionary4D.
                if (!globalBurletsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalBurletsDictionary3D.Add(obj.OrderInfo, ToDictionary2D(obj.dictBurlet));
                else
                {
                    foreach(var dict in obj.dictBurlet)
                    {
                        if (!globalBurletsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalBurletsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach(var item in dict.Value)
                            {
                                if (!globalBurletsDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                    globalBurletsDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                else
                                    globalBurletsDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                }

                if (!globalBurletsDictionary4D.ContainsKey(obj.OrderInfo))
                    globalBurletsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, ToDictionary2D(obj.dictBurlet) } });
                else
                {
                    if (!globalBurletsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalBurletsDictionary4D[obj.OrderInfo].Add(obj.TableName, ToDictionary2D(obj.dictBurlet));
                    else
                    {
                        foreach (var dict in obj.dictBurlet)
                        {
                            if (!globalBurletsDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                globalBurletsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
                                {
                                    if (!globalBurletsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                        globalBurletsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalBurletsDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            wordApp = new WordApp();

            wordApp.AddDocument(globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D, globalPerimetrsMaterialsList3D,
                globalMainCompositionsDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D, globalNotStegCutsDictionary3D,
                globalBurletsDictionary3D, globalMattressesDictionary4D, globalPolyurethaneSheetsDictionary4D, globalPolyurethaneForPerimetrsDictionary4D, globalPerimetrsMaterialsList4D,
                globalMainCompositionsDictionary4D, globalUltrCutsDictionary4D, globalV16CutsDictionary4D, globalKaterCutsDictionary4D, globalNotStegCutsDictionary4D, globalBurletsDictionary4D);

            Dictionary<string, Dictionary<string, int>> ToDictionary2D(Dictionary<string, Dictionary<string, int>> dictionary)
            {
                Dictionary<string, Dictionary<string, int>> tempDictionary = new Dictionary<string, Dictionary<string, int>>();
                foreach(var dict in dictionary)
                {
                    Dictionary<string, int> tempInnerDictionary = new Dictionary<string, int>();
                    foreach (var item in dict.Value)
                        tempInnerDictionary.Add(item.Key, item.Value);

                    tempDictionary.Add(dict.Key, tempInnerDictionary);
                }
                return tempDictionary;
            }

            Dictionary<string, int> ToDictionary(Dictionary<string, int> dictionary)
            {
                Dictionary<string, int> tempDictionary = new Dictionary<string, int>();
                foreach (var item in dictionary)
                    tempDictionary.Add(item.Key, item.Value);

                return tempDictionary;
            }

            #region Fields Clearing.
            globalTypesList.Clear();

            globalPolyurethaneSheetsDictionary3D.Clear();
            globalPolyurethaneSheetsDictionary4D.Clear();

            globalPolyurethaneForPerimetrsDictionary3D.Clear();
            globalPolyurethaneForPerimetrsDictionary4D.Clear();

            globalMainCompositionsDictionary3D.Clear();
            globalMainCompositionsDictionary4D.Clear();

            #region Cuts Dictionaries.
            globalUltrCutsDictionary3D.Clear();
            globalUltrCutsDictionary4D.Clear();

            globalV16CutsDictionary3D.Clear();
            globalV16CutsDictionary4D.Clear();

            globalKaterCutsDictionary3D.Clear();
            globalKaterCutsDictionary4D.Clear();

            globalNotStegCutsDictionary3D.Clear();
            globalNotStegCutsDictionary4D.Clear();
            #endregion

            globalBurletsDictionary3D.Clear();
            globalBurletsDictionary4D.Clear();
            #endregion

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
