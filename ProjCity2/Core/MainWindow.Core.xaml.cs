using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using EntityModels;
using DictionaryExtensions;

namespace ProjCity2
{
    public partial class MainWindow
    {
        private WordApp wordApp;
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

        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalUltrCoversDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalV16CoversDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalKaterCoversDictionary3D;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> globalNotStegCoversDictionary3D;

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

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalUltrCoversDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalV16CoversDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalKaterCoversDictionary4D;
        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalNotStegCoversDictionary4D;

        private Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> globalBurletsDictionary4D;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            using (PgContext context = new PgContext())
            {
                foreach (Mattresses mtrs in context.Mattresses)
                    listBoxMattressList.Items.Add(mtrs);
                foreach (Sizes size in context.Sizes)
                    cmbSizes.Items.Add(size);
                foreach (Series series in context.Series)
                    cmbSeries.Items.Add(series);
                foreach (Tables table in context.Tables)
                    cmbTables.Items.Add(table);
            }
        }

        #region Methods.

        #region Core Methods.
        private void AddMattressObject()
        {
            if (!(txtOrderId.Text.Length > 0 & txtDateOfOrder.Text.Length > 0))
                throw new Exception("Поля кода или даты заказа не должны быть пусты.");
            if (cmbTables.SelectedItem == null)
                throw new Exception("Не выбран стол сборки.");
            if(listBoxMattressList.SelectedItem == null)
                throw new Exception("Не выбран матрас.");
            string tableName = cmbTables.SelectedItem.ToString();

            int numbers = 1;
            if (txtNumbers.Text.Length != 0)
                numbers = Convert.ToInt32(txtNumbers.Text);

            int lenght, width;
            if (!(cmbSizes.SelectedItem == null & (txtCustomLenght.Text.Length == 0 & txtCustomWidth.Text.Length == 0)))
            {       
                if (cmbSizes.SelectedItem != null)
                {
                    Sizes tempSize = (Sizes)cmbSizes.SelectedItem;
                    lenght = tempSize.lenght;
                    width = tempSize.width;
                }
                else
                {
                    if (txtCustomLenght.Text.Length != 0 & txtCustomWidth.Text.Length != 0)
                    {
                        lenght = Convert.ToInt32(txtCustomLenght.Text);
                        width = Convert.ToInt32(txtCustomWidth.Text);
                    }
                    else
                        throw new Exception("Не указана длинна или ширина матраса.");
                }  
            }
            else
                throw new Exception("Отсутствуют данные о размере.");

            if (globalTypesList == null)
                globalTypesList = new List<MattressObjectV2>();

            MattressObjectV2 tempMattressObject = new MattressObjectV2(txtOrderId.Text + " : " + txtDateOfOrder.Text, tableName, (Mattresses)listBoxMattressList.SelectedItem, lenght, width, numbers);

            if (globalTypesList.Contains(tempMattressObject))
            {
                numbers += globalTypesList.Find(mattress => mattress == tempMattressObject).Numbers;
                globalTypesList.Remove(tempMattressObject);
                listBoxTypesList.Items.Remove(tempMattressObject);
                tempMattressObject = new MattressObjectV2(txtOrderId.Text + " : " + txtDateOfOrder.Text, tableName, (Mattresses)listBoxMattressList.SelectedItem, lenght, width, numbers);
            }

            globalTypesList.Add(tempMattressObject);
            listBoxTypesList.Items.Add(tempMattressObject);
        }

        private void CreateTotalOrderDocument()
        {
            #region Initializing Dictionaries.
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

            #region Initailizing Cuts Dictionaries.
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
            #endregion

            #region Initializing Covers Dictionaries.
            if (globalUltrCoversDictionary3D == null)
                globalUltrCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalUltrCoversDictionary4D == null)
                globalUltrCoversDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalV16CoversDictionary3D == null)
                globalV16CoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalV16CoversDictionary4D == null)
                globalV16CoversDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalKaterCoversDictionary3D == null)
                globalKaterCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalKaterCoversDictionary4D == null)
                globalKaterCoversDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

            if (globalNotStegCoversDictionary3D == null)
                globalNotStegCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalNotStegCoversDictionary4D == null)
                globalNotStegCoversDictionary4D = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();
            #endregion

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

                if (obj.GetCountOfPolyurethaneSheetDictionary() != 0)
                {
                    globalPolyurethaneSheetsDictionary3D.Unite(obj.OrderInfo, obj.GetPolyurethaneSheetsDictionary());
                    globalPolyurethaneSheetsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetPolyurethaneSheetsDictionary());
                }

                #region globalPolyurethaneForPerimetrsDictionaries.
                if (obj.GetCountOfPolyurethaneForPerimetrDictionary() != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempPolyurethaneForPerimetrDict = obj.GetPolyurethaneForPerimetrDictionary();

                    if (!globalPolyurethaneForPerimetrsDictionary3D.ContainsKey(obj.OrderInfo))
                    {
                        globalPolyurethaneForPerimetrsDictionary3D.Add(obj.OrderInfo, obj.GetPolyurethaneForPerimetrDictionary());

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
                                globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo].Add(dict.Key, dict.Value.CopyDictionary());

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
                        globalPolyurethaneForPerimetrsDictionary4D.Add(obj.OrderInfo, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { obj.TableName, obj.GetPolyurethaneForPerimetrDictionary()} });

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
                            globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo].Add(obj.TableName, obj.GetPolyurethaneForPerimetrDictionary());

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
                                    globalPolyurethaneForPerimetrsDictionary4D[obj.OrderInfo][obj.TableName].Add(dict.Key, dict.Value.CopyDictionary());

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

                if (obj.GetCountOfMainCompositionDictionary()!= 0)
                {
                    globalMainCompositionsDictionary3D.Unite(obj.OrderInfo, obj.GetMainCompositionDictionary());
                    globalMainCompositionsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetMainCompositionDictionary());
                }

                if (obj.GetCountOfBlockDictionary() != 0)
                {
                    globalBlocksDictionary3D.Unite(obj.OrderInfo, obj.GetBlocksDictionary());
                    globalBlocksDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetBlocksDictionary());
                }

                #region Cuts Dictionaries.
                if (obj.GetCountOfUltrCutDictionary() != 0)
                {
                    globalUltrCutsDictionary3D.Unite(obj.OrderInfo, obj.GetUltrCutDictionary());
                    globalUltrCutsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetUltrCutDictionary());
                }

                if (obj.GetCountOfV16CutDictionary() != 0)
                {
                    globalV16CutsDictionary3D.Unite(obj.OrderInfo, obj.GetV16CutDictionary());
                    globalV16CutsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetV16CutDictionary());
                }

                if (obj.GetCountOfKaterCutDictionary() != 0)
                {
                    globalKaterCutsDictionary3D.Unite(obj.OrderInfo, obj.GetKaterCutDictionary());
                    globalKaterCutsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetKaterCutDictionary());
                }

                if (obj.GetCountOfNotStegCutDictionry() != 0)
                {
                    globalNotStegCutsDictionary3D.Unite(obj.OrderInfo, obj.GetNotStegCutDictionary());
                    globalNotStegCutsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetNotStegCutDictionary());
                }
                #endregion

                #region Covers Dictionaries.
                //...
                #endregion

                if (obj.GetCountOfBurletDictionary() != 0)
                {
                    globalBurletsDictionary3D.Unite(obj.OrderInfo, obj.GetBurletDictionary());
                    globalBurletsDictionary4D.Unite(obj.OrderInfo, obj.TableName, obj.GetBurletDictionary());
                }
            }

            wordApp = new WordApp();

            wordApp.AddDocument(globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D, globalPerimetrsMaterialsList3D,
                globalMainCompositionsDictionary3D, globalBlocksDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D, globalNotStegCutsDictionary3D,
                globalBurletsDictionary3D, globalMattressesDictionary4D, globalPolyurethaneSheetsDictionary4D, globalPolyurethaneForPerimetrsDictionary4D, globalPerimetrsMaterialsList4D,
                globalMainCompositionsDictionary4D, globalBlocksDictionary4D, globalUltrCutsDictionary4D, globalV16CutsDictionary4D, globalKaterCutsDictionary4D, globalNotStegCutsDictionary4D, globalBurletsDictionary4D);

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
            txtOrderId.Clear();
            txtDateOfOrder.Clear();

            listBoxMattressList.SelectedItem = null;
            cmbSizes.SelectedItem = null;
            cmbTables.SelectedItem = null;
        }
        
        private void CreateMainOrderDocument()
        {
            #region Initializing Dictionaries.
            if (globalOrdersList == null)
                globalOrdersList = new List<string>();

            if (globalPolyurethaneSheetsDictionary3D == null)
                globalPolyurethaneSheetsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalPolyurethaneForPerimetrsDictionary3D == null)
                globalPolyurethaneForPerimetrsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            if (globalPerimetrsMaterialsList3D == null)
                globalPerimetrsMaterialsList3D = new Dictionary<string, List<string>>();

            if (globalMainCompositionsDictionary3D == null)
                globalMainCompositionsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalBlocksDictionary3D == null)
                globalBlocksDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            #region Cuts.
            if (globalUltrCutsDictionary3D == null)
                globalUltrCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalV16CutsDictionary3D == null)
                globalV16CutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalKaterCutsDictionary3D == null)
                globalKaterCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalNotStegCutsDictionary3D == null)
                globalNotStegCutsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            #endregion

            #region Covers.
            if (globalUltrCoversDictionary3D == null)
                globalUltrCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalV16CoversDictionary3D == null)
                globalV16CoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalKaterCoversDictionary3D == null)
                globalKaterCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

            if (globalNotStegCoversDictionary3D == null)
                globalNotStegCoversDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            #endregion

            if (globalBurletsDictionary3D == null)
                globalBurletsDictionary3D = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
            #endregion

            if (globalOrdersList == null)
                globalOrdersList = new List<string>();

            foreach(var obj in globalTypesList)
            {
                if (!globalOrdersList.Contains(obj.OrderInfo))
                    globalOrdersList.Add(obj.OrderInfo);

                if (obj.GetCountOfPolyurethaneSheetDictionary() != 0)
                    globalPolyurethaneSheetsDictionary3D.Unite(obj.OrderInfo, obj.GetPolyurethaneSheetsDictionary());
                #region Inserting Perimetr Dictionary
                if (obj.GetCountOfPolyurethaneForPerimetrDictionary() != 0)
                {
                    Dictionary<string, Dictionary<string, int>> tempPolyurethaneForPerimetrDict = obj.GetPolyurethaneForPerimetrDictionary();

                    if (!globalPolyurethaneForPerimetrsDictionary3D.ContainsKey(obj.OrderInfo))
                    {
                        globalPolyurethaneForPerimetrsDictionary3D.Add(obj.OrderInfo, obj.GetPolyurethaneForPerimetrDictionary());

                        globalPerimetrsMaterialsList3D.Add(obj.OrderInfo, new List<string> { });
                        foreach (var dict in tempPolyurethaneForPerimetrDict)
                            foreach (var item in dict.Value)
                                if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                    globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);
                    }
                    else
                    {
                        foreach (var dict in tempPolyurethaneForPerimetrDict)
                        {
                            if (!globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo].ContainsKey(dict.Key))
                            {
                                globalPolyurethaneForPerimetrsDictionary3D[obj.OrderInfo].Add(dict.Key, dict.Value.CopyDictionary());

                                foreach (var item in dict.Value)
                                    if (!globalPerimetrsMaterialsList3D[obj.OrderInfo].Contains(item.Key))
                                        globalPerimetrsMaterialsList3D[obj.OrderInfo].Add(item.Key);
                            }
                            else
                            {
                                foreach (var item in dict.Value)
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
                }
                #endregion
                if (obj.GetCountOfMainCompositionDictionary() != 0)
                    globalMainCompositionsDictionary3D.Unite(obj.OrderInfo, obj.GetMainCompositionDictionary());
                if (obj.GetCountOfBlockDictionary() != 0)
                    globalBlocksDictionary3D.Unite(obj.OrderInfo, obj.GetBlocksDictionary());
                #region Inserting Cuts Dictionaries.

                if (obj.GetCountOfUltrCutDictionary() != 0)
                    globalUltrCutsDictionary3D.Unite(obj.OrderInfo, obj.GetUltrCutDictionary());
                if (obj.GetCountOfV16CutDictionary() != 0)
                    globalV16CutsDictionary3D.Unite(obj.OrderInfo, obj.GetV16CutDictionary());
                if (obj.GetCountOfKaterCutDictionary() != 0)
                    globalKaterCutsDictionary3D.Unite(obj.OrderInfo, obj.GetKaterCutDictionary());
                if (obj.GetCountOfNotStegCutDictionry() != 0)
                    globalNotStegCutsDictionary3D.Unite(obj.OrderInfo, obj.GetNotStegCutDictionary());

                #endregion
                #region Inserting Covers Dictionaries.
                if (obj.GetCountOfUltrCoverDictionary() != 0)
                    globalUltrCoversDictionary3D.Unite(obj.OrderInfo, obj.GetUltrCoverDictionary());
                if (obj.GetCountOfV16CoverDictionary() != 0)
                    globalV16CoversDictionary3D.Unite(obj.OrderInfo, obj.GetV16CoverDictionary());
                if (obj.GetCountOfKaterCoverDictionary() != 0)
                    globalKaterCoversDictionary3D.Unite(obj.OrderInfo, obj.GetKaterCoverDictionary());
                if (obj.GetCountOfNotStegCoverDictionary() != 0)
                    globalNotStegCoversDictionary3D.Unite(obj.OrderInfo, obj.GetNotStegCoverDictionary());
                #endregion
                if (obj.GetCountOfBurletDictionary() != 0)
                    globalBurletsDictionary3D.Unite(obj.OrderInfo, obj.GetBurletDictionary());
            }
            wordApp = new WordApp();
            wordApp.AddDocument(globalOrdersList, globalPolyurethaneSheetsDictionary3D, globalPolyurethaneForPerimetrsDictionary3D, globalPerimetrsMaterialsList3D,
                globalMainCompositionsDictionary3D, globalBlocksDictionary3D, globalUltrCutsDictionary3D, globalV16CutsDictionary3D, globalKaterCutsDictionary3D,
                globalNotStegCutsDictionary3D, globalBurletsDictionary3D);

            #region Dictionaries Clearing.
            globalTypesList.Clear();
            globalPolyurethaneSheetsDictionary3D.Clear();
            globalPolyurethaneForPerimetrsDictionary3D.Clear();
            globalMainCompositionsDictionary3D.Clear();
            #region Cuts Dictionaries.

            globalUltrCutsDictionary3D.Clear();
            globalV16CutsDictionary3D.Clear();
            globalKaterCutsDictionary3D.Clear();
            globalNotStegCutsDictionary3D.Clear();

            #endregion
            #region Covers Dictionaries.
            globalUltrCoversDictionary3D.Clear();
            globalV16CoversDictionary3D.Clear();
            globalKaterCoversDictionary3D.Clear();
            globalNotStegCoversDictionary3D.Clear();
            #endregion
            globalBurletsDictionary3D.Clear();
            #endregion

            listBoxTypesList.Items.Clear();

            txtCustomLenght.Clear();
            txtCustomWidth.Clear();
            txtNumbers.Clear();
            txtOrderId.Clear();
            txtDateOfOrder.Clear();

            listBoxMattressList.SelectedItem = null;
            cmbSizes.SelectedItem = null;
            cmbTables.SelectedItem = null;
        }
        #endregion

        #region Methds For Edit Objects.
        public List<string> GetOrdersList()
        {
            List<string> listForReturn = new List<string>();
            foreach (var obj in globalTypesList)
                if (!listForReturn.Contains(obj.OrderInfo))
                    listForReturn.Add(obj.OrderInfo);
            return listForReturn;
        }

        public void RemoveGlobalTypesListObject(MattressObjectV2 objForRemove)
        {
            globalTypesList.Remove(objForRemove);
            listBoxTypesList.Items.Remove(objForRemove);
        }

        public void AddObjectInGlobalTypesList(MattressObjectV2 objForAdding)
        {
            if(globalTypesList.Contains(objForAdding))
            {
                globalTypesList.Remove(objForAdding);
                listBoxTypesList.Items.Remove(objForAdding);
            }
            globalTypesList.Add(objForAdding);
            listBoxTypesList.Items.Add(objForAdding);
        }
        #endregion

        #endregion
    }
}
