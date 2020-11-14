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

        //private List<MattressObject> globalTypesList;
        private List<MattressObjectV2> globalTypesList;

        #region Fields of Main Order: 3D-Dictionaries (Order Id str/( Composition str/( Size str/ Numbers)))
        private List<string> globalOrdersList;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneSheetsDictionary3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalPolyurethaneForPerimetrsDictionary3D;
        private Dictionary<string, List<string>> globalPerimetrsMaterialsList3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalMainCompositionsDictionary3D;

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalBlocksDictionary3D;

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

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBlocksDictionary4D;

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
            {
                txtNumbers.Clear();
                throw new Exception("Недопустимое значение (Ноль).");
            }
                

            int? tempLenght = null, tempWidth = null;
            if (txtCustomLenght.Text.Length != 0 & txtCustomWidth.Text.Length != 0)
            {
                tempLenght = Convert.ToInt32(txtCustomLenght.Text);
                tempWidth = Convert.ToInt32(txtCustomWidth.Text);
            }

            if (globalTypesList == null)
                globalTypesList = new List<MattressObjectV2>();

            //MattressObject tempMattressObject = new MattressObject((Mattresses)listBoxMattressList.SelectedItem, (Sizes)cmbSizes.SelectedItem, tempLenght, tempWidth, tempNumbers, txtOrderId.Text+" : " + txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem);
            MattressObjectV2 tempMattressObject = new MattressObjectV2(txtOrderId.Text + " : " + txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem, (Mattresses)listBoxMattressList.SelectedItem, tempLenght, tempWidth, (Sizes)cmbSizes.SelectedItem, tempNumbers);

            if (globalTypesList.Count != 0)
            {
                foreach (MattressObjectV2 item in globalTypesList)
                {
                    if (item.CompareTo(tempMattressObject))
                    {
                        tempNumbers += item.Numbers;
                        globalTypesList.Remove(item);
                        tempMattressObject = new MattressObjectV2(txtOrderId.Text + " : " + txtDateOfOrder.Text, (Tables)cmbTables.SelectedItem, (Mattresses)listBoxMattressList.SelectedItem, tempLenght, tempWidth, (Sizes)cmbSizes.SelectedItem, tempNumbers);
                        break;
                    }
                }
            }

            globalTypesList.Add(tempMattressObject);

            listBoxTypesList.Items.Clear();
            foreach (MattressObjectV2 obj in globalTypesList)
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

            if (globalBlocksDictionary3D == null)
                globalBlocksDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalBlocksDictionary4D == null)
                globalBlocksDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

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

            foreach(MattressObjectV2 obj in globalTypesList)
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

                #region globalPolyurethaneSheetsDictionaries.
                Dictionary<string, Dictionary<string, int>> tempPolyurethaneDict = obj.GetPolyurethaneSheetsDictionary();

                if (!globalPolyurethaneSheetsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalPolyurethaneSheetsDictionary3D.Add(obj.OrderInfo, obj.GetPolyurethaneSheetsDictionary()); //obj.GetPolyurethaneSheetsDictionary();
                else
                {
                    foreach (var dict in tempPolyurethaneDict)
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
                    globalPolyurethaneSheetsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetPolyurethaneSheetsDictionary() } }); //obj.GetPolyurethaneSheetsDictionary();
                else
                {
                    if (!globalPolyurethaneSheetsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalPolyurethaneSheetsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetPolyurethaneSheetsDictionary()); //obj.GetPolyurethaneSheetsDictionary();
                    else
                    {
                        foreach (var dict in tempPolyurethaneDict)
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
                if (obj.GetPolyurethaneForPerimetrDictionary().Count != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempPolyurethaneForPerimetrDict = obj.GetPolyurethaneForPerimetrDictionary();

                    if (!globalPolyurethaneForPerimetrsDictionary3D.ContainsKey(obj.OrderInfo))
                    {
                        globalPolyurethaneForPerimetrsDictionary3D.Add(obj.OrderInfo, obj.GetPolyurethaneForPerimetrDictionary()); //obj.GetPolyurethaneForPerimetrDictionary();

                        globalPerimetrsMaterialsList3D.Add(obj.OrderInfo, new List<string> { });
                        foreach (var dict in tempPolyurethaneForPerimetrDict)
                            foreach (var item in dict.Value)
                                if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                    globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);
                    }   
                    else
                    {
                        foreach(var dict in tempPolyurethaneForPerimetrDict)
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
                        globalPolyurethaneForPerimetrsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetPolyurethaneForPerimetrDictionary()} }); //obj.GetPolyurethaneForPerimetrDictionary();

                        globalPerimetrsMaterialsList4D.Add(obj.OrderInfo, new Dictionary<string, List<string>> { { obj.TableName, new List<string> { } } });
                        foreach (var dict in tempPolyurethaneForPerimetrDict)
                            foreach (var item in dict.Value)
                                if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                    globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);
                    }
                    else
                    {
                        if (!globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        {
                            globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetPolyurethaneForPerimetrDictionary()); //obj.GetPolyurethaneForPerimetrDictionary();

                            globalPerimetrsMaterialsList4D[obj.OrderInfo].Add(obj.TableName, new List<string> { });
                            foreach (var dict in tempPolyurethaneForPerimetrDict)
                                foreach (var item in dict.Value)
                                    if (!globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Contains(item.Key))
                                        globalPerimetrsMaterialsList4D[obj.OrderInfo][obj.TableName].Add(item.Key);
                        }
                        else
                        {
                            foreach (var dict in tempPolyurethaneForPerimetrDict)
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

                // !!!
                #region globalMainCompositionsDictionaries.
                Dictionary<string, Dictionary<string, int>> tempMainCompositionDict = obj.GetMainCompositionDictionary();

                if (!globalMainCompositionsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalMainCompositionsDictionary3D.Add(obj.OrderInfo, obj.GetMainCompositionDictionary()); //obj.GetMainCompositionDictionary();
                else
                {
                    foreach (var dict in tempMainCompositionDict)
                    {
                        if (!globalMainCompositionsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalMainCompositionsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach (var item in dict.Value)
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
                    globalMainCompositionsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetMainCompositionDictionary() } }); //obj.GetMainCompositionDictionary();
                else
                {
                    if (!globalMainCompositionsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalMainCompositionsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetMainCompositionDictionary()); //obj.GetMainCompositionDictionary();
                    else
                    {
                        foreach (var dict in tempMainCompositionDict)
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

                // !!!
                #region globalBlocksDictionaries. 
                Dictionary<string, Dictionary<string, int>> tempBlockDict = obj.GetBlocksDictionary();

                if (!globalBlocksDictionary3D.ContainsKey(obj.OrderInfo))
                    globalBlocksDictionary3D.Add(obj.OrderInfo, obj.GetBlocksDictionary());
                else
                {
                    foreach (var dict in tempBlockDict)
                    {
                        if (!globalBlocksDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalBlocksDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach (var item in dict.Value)
                            {
                                if (!globalBlocksDictionary3D[obj.OrderInfo][dict.Key].ContainsKey(item.Key))
                                    globalBlocksDictionary3D[obj.OrderInfo][dict.Key].Add(item.Key, item.Value);
                                else
                                    globalBlocksDictionary3D[obj.OrderInfo][dict.Key][item.Key] += item.Value;
                            }
                        }
                    }
                }

                if (!globalBlocksDictionary4D.ContainsKey(obj.OrderInfo))
                    globalBlocksDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetBlocksDictionary() } });
                else
                {
                    if (!globalBlocksDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalBlocksDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetBlocksDictionary());
                    else
                    {
                        foreach (var dict in tempBlockDict)
                        {
                            if (!globalBlocksDictionary4D[obj.OrderInfo][obj.TableName].ContainsKey(dict.Key))
                                globalBlocksDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
                                {
                                    if (!globalBlocksDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].ContainsKey(item.Key))
                                        globalBlocksDictionary4D[obj.OrderInfo][obj.TableName][dict.Key].Add(item.Key, item.Value);
                                    else
                                        globalBlocksDictionary4D[obj.OrderInfo][obj.TableName][dict.Key][item.Key] += item.Value;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Cuts Dictionaries.
                #region globalUltrCutsDictionary4D.
                if (obj.GetUltrCutDictionary().Count != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempUltrCutDict = obj.GetUltrCutDictionary();

                    if (!globalUltrCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalUltrCutsDictionary3D.Add(obj.OrderInfo, obj.GetUltrCutDictionary()); //obj.GetUltrCutDictionary();
                    else
                    {
                        foreach (var dict in tempUltrCutDict)
                        {
                            if (!globalUltrCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalUltrCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
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
                        globalUltrCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetUltrCutDictionary() } }); //obj.GetUltrCutDictionary();
                    else
                    {
                        if (!globalUltrCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalUltrCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetUltrCutDictionary()); //obj.GetUltrCutDictionary();
                        else
                        {
                            foreach (var dict in tempUltrCutDict)
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
                if (obj.GetV16CutDictionary().Count != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempV16CutDuct = obj.GetV16CutDictionary();

                    if (!globalV16CutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalV16CutsDictionary3D.Add(obj.OrderInfo, obj.GetV16CutDictionary()); //obj.GetV16CutDictionary();
                    else
                    {
                        foreach (var dict in tempV16CutDuct)
                        {
                            if (!globalV16CutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalV16CutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
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
                        globalV16CutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetV16CutDictionary() } }); //obj.GetV16CutDictionary();
                    else
                    {
                        if (!globalV16CutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalV16CutsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetV16CutDictionary()); //obj.GetV16CutDictionary();
                        else
                        {
                            foreach (var dict in tempV16CutDuct)
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
                if (obj.GetKaterCutDictionary().Count != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempKaterCutDict = obj.GetKaterCutDictionary();

                    if (!globalKaterCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalKaterCutsDictionary3D.Add(obj.OrderInfo, obj.GetKaterCutDictionary()); //obj.GetKaterCutDictionary();
                    else
                    {
                        foreach (var dict in tempKaterCutDict)
                        {
                            if (!globalKaterCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalKaterCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
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
                        globalKaterCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetKaterCutDictionary() } }); //obj.GetKaterCutDictionary();
                    else
                    {
                        if (!globalKaterCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalKaterCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetKaterCutDictionary()); //obj.GetKaterCutDictionary();
                        else
                        {
                            foreach (var dict in tempKaterCutDict)
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
                if (obj.GetNotStegCutDictionary().Count != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempNotStegCutDict = obj.GetNotStegCutDictionary();

                    if (!globalNotStegCutsDictionary3D.ContainsKey(obj.OrderInfo))
                        globalNotStegCutsDictionary3D.Add(obj.OrderInfo, obj.GetNotStegCutDictionary()); //obj.GetNotStegCutDictionary()
                    else
                    {
                        foreach (var dict in tempNotStegCutDict)
                        {
                            if (!globalNotStegCutsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                                globalNotStegCutsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                            else
                            {
                                foreach (var item in dict.Value)
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
                        globalNotStegCutsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetNotStegCutDictionary() } }); //obj.GetNotStegCutDictionary()
                    else
                    {
                        if (!globalNotStegCutsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                            globalNotStegCutsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetNotStegCutDictionary()); //obj.GetNotStegCutDictionary()
                        else
                        {
                            foreach (var dict in tempNotStegCutDict)
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
                Dictionary<string, Dictionary<string, int>> tempBurletDict = obj.GetBurletDictionary();

                if (!globalBurletsDictionary3D.ContainsKey(obj.OrderInfo))
                    globalBurletsDictionary3D.Add(obj.OrderInfo, obj.GetBurletDictionary()); //obj.GetBurletDictionary();
                else
                {
                    foreach (var dict in tempBurletDict)
                    {
                        if (!globalBurletsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            globalBurletsDictionary3D[obj.OrderInfo].Add(dict.Key, ToDictionary(dict.Value));
                        else
                        {
                            foreach (var item in dict.Value)
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
                    globalBurletsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetBurletDictionary() } }); //obj.GetBurletDictionary();
                else
                {
                    if (!globalBurletsDictionary4D[obj.OrderInfo].ContainsKey(obj.TableName))
                        globalBurletsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetBurletDictionary()); //obj.GetBurletDictionary();
                    else
                    {
                        foreach (var dict in tempBurletDict)
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
                globalMainCompositionsDictionary3D, globalBlocksDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D, globalNotStegCutsDictionary3D,
                globalBurletsDictionary3D, globalMattressesDictionary4D, globalPolyurethaneSheetsDictionary4D, globalPolyurethaneForPerimetrsDictionary4D, globalPerimetrsMaterialsList4D,
                globalMainCompositionsDictionary4D, globalBlocksDictionary4D, globalUltrCutsDictionary4D, globalV16CutsDictionary4D, globalKaterCutsDictionary4D, globalNotStegCutsDictionary4D, globalBurletsDictionary4D);

            Dictionary<string, int> ToDictionary(Dictionary<string, int> dictionary)
            {
                Dictionary<string, int> tempDictionary = new Dictionary<string, int>();
                foreach (var item in dictionary)
                    tempDictionary.Add(item.Key, item.Value);

                return tempDictionary;
            }

            #region Dictionaries Clearing.
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
