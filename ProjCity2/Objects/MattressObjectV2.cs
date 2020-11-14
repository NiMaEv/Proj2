using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EntityModels;

namespace ProjCity2
{
    public class MattressObjectV2
    {
        public string OrderInfo { get; }
        public string TableName { get; }
        public string Name { get; }

        private int lenght;
        private int width;
        public string Size { get => $"{lenght}*{width}"; }
        private string SizeForComponents { get; }
        private string SizeForBlocks { get; }
        public int Numbers { get; }

        #region Dictionaryies.
        private Dictionary<string, Dictionary<string, int>> dictPolyurethaneSheet;
        private Dictionary<string, Dictionary<string, int>> dictPolyurethaneForPerimetr;

        private Dictionary<string, Dictionary<string, int>> dictMainComposition;
        private Dictionary<string, Dictionary<string, int>> dictBlocks;

        private Dictionary<string, Dictionary<string, int>> dictBurlet;

        private Dictionary<string, Dictionary<string, int>> dictUltrCut;
        private Dictionary<string, Dictionary<string, int>> dictKaterCut;
        private Dictionary<string, Dictionary<string, int>> dictV16Cut;
        private Dictionary<string, Dictionary<string, int>> dictNotStegCut;
        #endregion

        public MattressObjectV2(string orderInfo, Tables table, Mattresses mattress, int? customLenght, int? customWidth, Sizes size, int numbers)
        {
            #region Initialize of Fields.
            if (orderInfo.Length == 0)
                throw new Exception("Поля кода или даты заказа не должны быть пусты.");
            OrderInfo = orderInfo;
            if (table == null)
                throw new Exception("Не выбран стол сборки.");
            TableName = table.tableName;
            if (mattress == null)
                throw new Exception("Не выбран матрас.");
            Name = mattress.mattressName;

            if ((customLenght == null | customWidth == null) & size == null)
                throw new Exception("Данные о размере не верны.");
            if (size == null)
            {
                lenght = (int)customLenght;
                width = (int)customWidth;
            }
            else
            {
                lenght = size.lenght;
                width = size.width;
            }

            SizeForBlocks = SizeForComponents = Size;

            Numbers = numbers;
            #endregion

            using (PgContext context = new PgContext())
            {
                MtrsCompositions mainComposition = context.MtrsCompositions.Find(mattress.compositionId);
                if (mainComposition.topSideCompositionId == null & mainComposition.botSideCompositionId == null & mainComposition.generalComposition == null)
                    throw new Exception($"Неверный формат записи.\nMtrsCompositions/compositionId({mainComposition.compositionId}).");
                dictMainComposition = new Dictionary<string, Dictionary<string, int>>();

                #region Inserting in dictPolyurethaneForPerimetr.
                if (mattress.perimetrId != null)
                {
                    Perimetrs perimetr = context.Perimetrs.Find(mattress.perimetrId);
                    if (perimetr.reinforcmentBlockMaterialName != null | perimetr.reinforcmentMattressMaterialName != null)
                    {
                        string xLenght = $"{lenght}";
                        string yLenght = $"{width - (perimetr.reinforcmentMaterialWidth * 2)}";
                        dictPolyurethaneForPerimetr = new Dictionary<string, Dictionary<string, int>>();

                        dictPolyurethaneForPerimetr = new Dictionary<string, Dictionary<string, int>>();
                        if (perimetr.reinforcmentBlockMaterialName != null)
                        {
                            SizeForBlocks = $"{lenght - perimetr.reinforcmentMaterialWidth}*{width - perimetr.reinforcmentMaterialWidth}";

                            Blocks mainBlock = context.Blocks.Find(mainComposition.blockId);
                            Dictionary<string, int> tempDict = new Dictionary<string, int>() { { perimetr.reinforcmentBlockMaterialName +
                                    "h=" + mainBlock.blockHeight / 10, Numbers * 2 } };
                            dictPolyurethaneForPerimetr.Add(xLenght, tempDict);
                            dictPolyurethaneForPerimetr.Add(yLenght, tempDict);

                            if (mainComposition.additionalBlockId != null)
                            {
                                Blocks additionalBlock = context.Blocks.Find(mainComposition.additionalBlockId);
                                dictPolyurethaneForPerimetr[xLenght].Add(perimetr.reinforcmentBlockMaterialName +
                                    "h=" + additionalBlock.blockHeight / 10, Numbers * 2);
                                dictPolyurethaneForPerimetr[yLenght].Add(perimetr.reinforcmentBlockMaterialName +
                                    "h=" + additionalBlock.blockHeight / 10, Numbers * 2);
                            }
                        } 
                        if (perimetr.reinforcmentMattressMaterialName != null)
                        {
                            SizeForBlocks = SizeForComponents = $"{lenght - perimetr.reinforcmentMaterialWidth * 2}*{width - perimetr.reinforcmentMaterialWidth * 2}";

                            Dictionary<string, int> tempDict = new Dictionary<string, int>() { { perimetr.reinforcmentMattressMaterialName +
                                    "h=" + GetMattressHeight(mattress) / 10, Numbers * 2} };
                            dictPolyurethaneForPerimetr.Add(xLenght, tempDict);
                            dictPolyurethaneForPerimetr.Add(yLenght, tempDict);
                        }
                    }
                }
                #endregion

                #region Inserting in dictPolyurethaneSheet.
                string tempCompositionStr = null;
                if (mainComposition.generalComposition != null)
                    tempCompositionStr += context.MtrsCompositions.Find(mainComposition.compositionId).generalComposition;
                if (mainComposition.topSideCompositionId != null)
                    tempCompositionStr += context.MtrsCompositionSides.Find(mainComposition.topSideCompositionId).composition;
                if (mainComposition.botSideCompositionId != null)
                    tempCompositionStr += context.MtrsCompositionSides.Find(mainComposition.botSideCompositionId).composition;
                if (GetPolyurethaneSheetsDictionary(tempCompositionStr).Count != 0)
                {
                    dictPolyurethaneSheet = new Dictionary<string, Dictionary<string, int>>();
                    UnitDictionaries(dictPolyurethaneSheet, GetPolyurethaneSheetsDictionary(tempCompositionStr), SizeForComponents);
                }
                #endregion

                #region Inserting in dictMainComposition.
                if (mainComposition.topSideCompositionId != null & mainComposition.botSideCompositionId != null)
                {
                    MtrsCompositionSides compositionOfTopSide = context.MtrsCompositionSides.Find(mainComposition.topSideCompositionId);
                    if(mainComposition.topSideCompositionId == mainComposition.botSideCompositionId)
                    {
                        dictMainComposition.Add(compositionOfTopSide.composition, new Dictionary<string, int>() { { SizeForComponents, Numbers * 2 } });
                    }
                    else
                    {
                        MtrsCompositionSides compositionOfBotSide = context.MtrsCompositionSides.Find(mainComposition.botSideCompositionId);
                        dictMainComposition.Add(compositionOfTopSide.composition, new Dictionary<string, int>() { { SizeForComponents, Numbers } });
                        dictMainComposition.Add(compositionOfBotSide.composition, new Dictionary<string, int>() { { SizeForComponents, Numbers } });
                    }
                }
                if (mainComposition.generalComposition != null)
                {
                    dictMainComposition.Add(mainComposition.generalComposition, new Dictionary<string, int>() { { SizeForComponents, Numbers } });
                }
                #endregion

                #region Inserting in dictBlocks
                if (mainComposition.blockId != null)
                {
                    dictBlocks = new Dictionary<string, Dictionary<string, int>>();

                    Blocks mainBlock = context.Blocks.Find(mainComposition.blockId);
                    dictBlocks.Add(mainBlock.blockName, new Dictionary<string, int>() { { SizeForBlocks, Numbers } });
                    if (mainComposition.additionalBlockId != null)
                    {
                        Blocks additionalBlock = context.Blocks.Find(mainComposition.additionalBlockId);
                        dictBlocks.Add(additionalBlock.blockName, new Dictionary<string, int>() { { SizeForBlocks, Numbers } });
                    }
                }
                #endregion

                #region Inserting in Dictionaries of Cuts.
                Cuts mainCut = context.Cuts.Find(mattress.cutId);
                if (mainCut.topSideCompositionId == null & mainCut.botSideCompositionId == null & mainCut.cutCase == null)
                    throw new Exception($"Неверный формат записи.\nCuts/cutId({mainCut.cutId})");

                CutCompositionSides compositionOfTopSideCut = null;
                CutCompositionSides compositionOfBotSideCut = null;
                if (mainCut.topSideCompositionId != null & mainCut.botSideCompositionId != null)
                {
                    compositionOfTopSideCut = context.CutCompositionSides.Find(mainCut.topSideCompositionId);
                    compositionOfBotSideCut = context.CutCompositionSides.Find(mainCut.botSideCompositionId);
                }

                switch (mainCut.sectorName)
                {
                    case "Ультразвук":
                        dictUltrCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null)
                        {
                            if(mainCut.topSideCompositionId == mainCut.botSideCompositionId)
                                dictUltrCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            else
                            {
                                dictUltrCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                                dictUltrCut.Add(compositionOfBotSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                            }    
                        }
                        //Case (Cuts/cutCase).
                        DistributeDictionary(dictUltrCut);
                        dictMainComposition = DictionaryClear(dictMainComposition, dictUltrCut);
                        break;
                    case "V-16":
                        dictV16Cut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null)
                        {
                            if (mainCut.topSideCompositionId == mainCut.botSideCompositionId)
                                dictV16Cut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            else
                            {
                                dictV16Cut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                                dictV16Cut.Add(compositionOfBotSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                            }
                        }
                        //Case (Cuts/cutCase).
                        DistributeDictionary(dictV16Cut);
                        dictMainComposition = DictionaryClear(dictMainComposition, dictV16Cut);
                        break;
                    case "Катерман":
                        dictKaterCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null)
                        {
                            if (mainCut.topSideCompositionId == mainCut.botSideCompositionId)
                                dictKaterCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            else
                            {
                                dictKaterCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                                dictKaterCut.Add(compositionOfBotSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                            }
                        }
                        //Case (Cuts/cutCase).
                        DistributeDictionary(dictKaterCut);
                        dictMainComposition = DictionaryClear(dictMainComposition, dictKaterCut);
                        break;
                    case "Не стегается":
                        dictNotStegCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null)
                        {
                            if (mainCut.topSideCompositionId == mainCut.botSideCompositionId)
                                dictNotStegCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            else
                            {
                                dictNotStegCut.Add(compositionOfTopSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                                dictNotStegCut.Add(compositionOfBotSideCut.composition, new Dictionary<string, int>() { { Size, Numbers } });
                            }
                        }
                        //Case (Cuts/cutCase).
                        DistributeDictionary(dictNotStegCut);
                        dictMainComposition = DictionaryClear(dictMainComposition, dictNotStegCut);
                        break;
                }
                #endregion

                #region Inserting in dictBurlet.
                if (mattress.burletId != null)
                {
                    Burlets burlet = context.Burlets.Find(mattress.burletId);
                    dictBurlet = new Dictionary<string, Dictionary<string, int>>();
                    dictBurlet.Add(burlet.composition, new Dictionary<string, int>() { { Size, Numbers } });
                }
                #endregion

                if (dictPolyurethaneSheet != null)
                    dictMainComposition = DictionaryClear(dictMainComposition, dictPolyurethaneSheet);
            }
        }

        #region Methods.
        private Dictionary<string, int> GetPolyurethaneSheetsDictionary(string compositionStr)
        {
            Dictionary<string, int> tempDictionary = new Dictionary<string, int>();

            using (PgContext context = new PgContext())
            {
                if (compositionStr != null)
                {
                    string tempStr = null;
                    foreach (char i in compositionStr.ToArray())
                    {
                        if (i.ToString() != "/" && i.ToString() != ".")
                            tempStr += i;
                        else
                        {
                            if (context.Materials.Find(tempStr) != null && context.Materials.Find(tempStr).sectorName.Equals("ППУ"))
                            {
                                if (!tempDictionary.ContainsKey(tempStr))
                                    tempDictionary.Add(tempStr, Numbers);
                                else
                                    tempDictionary[tempStr] += Numbers;
                            }
                            tempStr = null;
                        }
                    }
                }
            }
            return tempDictionary;
        }

        private void UnitDictionaries(Dictionary<string, Dictionary<string, int>> currentDictionary, Dictionary<string, int> additionalDictionary, string size)
        {
            foreach(var item in additionalDictionary)
            {
                if (!currentDictionary.ContainsKey(item.Key))
                    currentDictionary.Add(item.Key, new Dictionary<string, int>() { { size, item.Value } });
                else
                {
                    if (!currentDictionary[item.Key].ContainsKey(size))
                        currentDictionary[item.Key].Add(size, Numbers * 2);
                    else
                        currentDictionary[item.Key][size] += item.Value;
                }
            }
        }

        private int GetMattressHeight(Mattresses mattress)
        {
            int height = 0;

            using (PgContext context = new PgContext())
            {
                string compositionStr = null;
                if (context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId) != null)
                    compositionStr += context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId).composition;
                if (context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId) != null)
                    compositionStr += context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId).composition;
                if (context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                    compositionStr += context.MtrsCompositions.Find(mattress.compositionId).generalComposition;

                string tempStr = null;

                foreach (char i in compositionStr.ToArray())
                {
                    if (i.ToString() != "/" && i.ToString() != ".")
                        tempStr += i;
                    else
                    {
                        if (context.Materials.Find(tempStr) != null && context.Materials.Find(tempStr).materialHeight != null)
                            height += (int)context.Materials.Find(tempStr).materialHeight;
                        tempStr = null;
                    }
                }
                if (context.MtrsCompositions.Find(mattress.compositionId).blockId != null)
                    height += (int)context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).blockId).blockHeight;
                if (context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId != null)
                    height += (int)context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId).blockHeight;
            }

            return height;
        }

        private Dictionary<string, Dictionary<string, int>> DictionaryClear(Dictionary<string, Dictionary<string, int>> currentDictionary, Dictionary<string, Dictionary<string, int>> dictionaryOfItemForDelete)
        {
            Dictionary<string, Dictionary<string, int>> returnedDictionary = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> tempDictionary = CopyDictionary(currentDictionary);

            List<string> itemsList = new List<string>();
            string tempStr = null;
            string newStrOfComposition = null;

            foreach(var item in tempDictionary)
            {
                foreach(char i in item.Key.ToArray())
                {
                    if (i.ToString() != "/" & i.ToString() != ".")
                        tempStr += i;
                    else
                    {
                        itemsList.Add(tempStr);
                        tempStr = null;
                    }
                }

                foreach (var item1 in dictionaryOfItemForDelete)
                    while (itemsList.Contains(item1.Key))
                        itemsList.Remove(item1.Key);

                if(itemsList.Count == 0)
                {
                    tempDictionary.Remove(item.Key);
                    break;
                }

                foreach (string str in itemsList)
                    newStrOfComposition += str + "/";
                itemsList.Clear();

                char[] arr = newStrOfComposition.ToArray();
                newStrOfComposition = null;

                for (int i = 0; i <= arr.Length - 1; i++)
                {
                    if (i != arr.Length - 1)
                        newStrOfComposition += arr[i];
                    else
                        newStrOfComposition += ".";
                }

                returnedDictionary.Add(newStrOfComposition, item.Value);

                newStrOfComposition = null;
            }
            return returnedDictionary;
        }

        private void DistributeDictionary(Dictionary<string, Dictionary<string, int>> cutDict)
        {
            using (PgContext context = new PgContext())
            {
                foreach (var item in dictMainComposition)
                    foreach (var itemIn in item.Value)
                    {
                        if (context.Materials.Find(item.Key) != null && context.Materials.Find(item.Key).sectorName.Equals("Крой"))
                        {
                            if (!cutDict.ContainsKey(item.Key))
                                cutDict.Add(item.Key, item.Value);
                            else
                            {
                                if (!cutDict[item.Key].ContainsKey(itemIn.Key))
                                    cutDict[item.Key].Add(itemIn.Key, itemIn.Value);
                                else
                                    cutDict[item.Key][itemIn.Key] += itemIn.Value;
                            }
                        }
                    }
            }
        }

        #region Methods of Gets Dictionaryies.
        private Dictionary<string, Dictionary<string, int>> CopyDictionary(Dictionary<string, Dictionary<string, int>> dictionaryForCopy)
        {
            Dictionary<string, Dictionary<string, int>> tempDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (dictionaryForCopy != null)
                foreach (var dict in dictionaryForCopy)
                {
                    Dictionary<string, int> tempInnerDictionary = new Dictionary<string, int>();
                    foreach (var item in dict.Value)
                        tempInnerDictionary.Add(item.Key, item.Value);

                    tempDictionary.Add(dict.Key, tempInnerDictionary);
                }
            return tempDictionary;
        }

        public Dictionary<string, Dictionary<string, int>> GetPolyurethaneSheetsDictionary() => CopyDictionary(dictPolyurethaneSheet);
        public Dictionary<string, Dictionary<string, int>> GetPolyurethaneForPerimetrDictionary() => CopyDictionary(dictPolyurethaneForPerimetr);
        public Dictionary<string, Dictionary<string, int>> GetMainCompositionDictionary() => CopyDictionary(dictMainComposition);
        public Dictionary<string, Dictionary<string, int>> GetBlocksDictionary() => CopyDictionary(dictBlocks);
        public Dictionary<string, Dictionary<string, int>> GetUltrCutDictionary() => CopyDictionary(dictUltrCut);
        public Dictionary<string, Dictionary<string, int>> GetV16CutDictionary() => CopyDictionary(dictV16Cut);
        public Dictionary<string, Dictionary<string, int>> GetKaterCutDictionary() => CopyDictionary(dictKaterCut);
        public Dictionary<string, Dictionary<string, int>> GetNotStegCutDictionary() => CopyDictionary(dictNotStegCut);
        public Dictionary<string, Dictionary<string, int>> GetBurletDictionary() => CopyDictionary(dictBurlet);
        #endregion

        public override string ToString() => $"{Name}\nЗаказ: {OrderInfo} \nСтол:{TableName}\nРазмер: {Size}\nКоличество: {Numbers}\n***************************************************";

        public bool CompareTo(MattressObjectV2 obj) => (this.Name.Equals(obj.Name) & this.OrderInfo.Equals(obj.OrderInfo) &
            this.TableName.Equals(obj.TableName) & this.Size.Equals(obj.Size)) ? true : false;
        #endregion
    }
}


