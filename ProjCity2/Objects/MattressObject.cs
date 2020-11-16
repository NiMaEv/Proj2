using EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjCity2
{
    public class MattressObject
    {
        public string Name { get; }

        private int lenght;
        private int width;
        public string Size { get => $"{lenght}*{width}"; }
        
        public int Numbers { get; }

        public string OrderInfo { get; }

        public string TableName { get; }

        #region Dictionaries.

        public Dictionary<string, Dictionary<string, int>> dictPolyurethaneSheet { get; }
        public Dictionary<string, Dictionary<string, int>> dictPolyurethaneForPerimetr { get; }

        public Dictionary<string, Dictionary<string, int>> dictMainComposition { get; }

        public Dictionary<string, Dictionary<string, int>> dictBurlet { get; }

        public Dictionary<string, Dictionary<string, int>> dictUltrCut { get; }
        public Dictionary<string, Dictionary<string, int>> dictKaterCut { get; }
        public Dictionary<string, Dictionary<string, int>> dictV16Cut { get; }
        public Dictionary<string, Dictionary<string, int>> dictNotStegCut { get; }
        #endregion

        public MattressObject(Mattresses mattress, Sizes size, int? customLenght, int? customWidth, int numbers, string orderInfo, Tables table)
        {
            dictPolyurethaneSheet = new Dictionary<string, Dictionary<string, int>>();
            dictPolyurethaneForPerimetr = new Dictionary<string, Dictionary<string, int>>();
            dictMainComposition = new Dictionary<string, Dictionary<string, int>>();
            dictBurlet = new Dictionary<string, Dictionary<string, int>>();

            using (PgContext context = new PgContext())
            {
                if (mattress == null)
                    throw new Exception("Не выбран элемент в списке матрасов.");
                Name = mattress.mattressName;

                if (size != null) 
                {
                    lenght = size.lenght;
                    width = size.width;
                }
                else
                {
                    lenght = (int)customLenght;
                    width = (int)customWidth;
                }

                Numbers = numbers;

                OrderInfo = orderInfo;
                TableName = table.tableName;

                if ((context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId != null && context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId != null) || context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                {
                    Dictionary<string, int> tempDict = null;
                    if(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId == null && context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId == null)
                        tempDict = GetPolyurethaneSheetsDictionary(context.MtrsCompositions.Find(mattress.compositionId).generalComposition);
                    else
                        tempDict = GetPolyurethaneSheetsDictionary(context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId).composition + context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId).composition + context.MtrsCompositions.Find(mattress.compositionId).generalComposition);
                    dictPolyurethaneSheet = UnitDictionaries(dictPolyurethaneSheet, tempDict, Size);
                }

                if (mattress.perimetrId != null && (context.Perimetrs.Find(mattress.perimetrId).reinforcmentMattressMaterialName != null || context.Perimetrs.Find(mattress.perimetrId).reinforcmentBlockMaterialName != null))
                {
                    string xLenght = $"l={lenght}";
                    string yLenght = $"l={width - ((int)context.Perimetrs.Find(mattress.perimetrId).reinforcmentMaterialWidth * 2)}";

                    string tempCompositionTopSide = null;
                    string tempCompositionBotSide = null;
                    string tempGeneralComposition = null;

                    if (context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId != null)
                        tempCompositionTopSide = context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId).composition;
                    if (context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId != null)
                        tempCompositionBotSide = context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId).composition;
                    if (context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                        tempGeneralComposition = context.MtrsCompositions.Find(mattress.compositionId).generalComposition;

                    if (context.Perimetrs.Find(mattress.perimetrId).reinforcmentMattressMaterialName != null)
                    {
                        Dictionary<string, int> tempDict = new Dictionary<string, int>() { { context.Perimetrs.Find(mattress.perimetrId).reinforcmentMattressMaterialName + " h=" + GetMattressHeight(mattress, tempCompositionTopSide + tempCompositionBotSide + tempGeneralComposition)/10, Numbers * 2 } };

                        dictPolyurethaneForPerimetr.Add(xLenght, tempDict);
                        dictPolyurethaneForPerimetr.Add(yLenght, tempDict);
                    }

                    if (context.Perimetrs.Find(mattress.perimetrId).reinforcmentBlockMaterialName != null)
                    {
                        if (context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId != null)
                        {
                            Dictionary<string, int> tempDict1 = new Dictionary<string, int>() { { context.Perimetrs.Find(mattress.perimetrId).reinforcmentBlockMaterialName + " h=" + context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId).blockHeight/10, Numbers * 2 } };

                            dictPolyurethaneForPerimetr = UnitDictionaries(dictPolyurethaneForPerimetr, tempDict1, xLenght);
                            dictPolyurethaneForPerimetr = UnitDictionaries(dictPolyurethaneForPerimetr, tempDict1, yLenght);
                        }

                        Dictionary<string, int> tempDict2 = new Dictionary<string, int>() { { context.Perimetrs.Find(mattress.perimetrId).reinforcmentBlockMaterialName + " h=" + context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).blockId).blockHeight/10, Numbers * 2 } };

                        dictPolyurethaneForPerimetr = UnitDictionaries(dictPolyurethaneForPerimetr, tempDict2, xLenght);
                        dictPolyurethaneForPerimetr = UnitDictionaries(dictPolyurethaneForPerimetr, tempDict2, yLenght);
                    }
                }

                if ((context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId != null && context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId != null) || context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                {
                    if(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId == null && context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId == null)
                    {
                        if (context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                        {
                            dictMainComposition.Add(context.MtrsCompositions.Find(mattress.compositionId).generalComposition, new Dictionary<string, int>() { { Size, Numbers } });
                        }     
                    }
                    else
                    {
                        if (context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId == context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId)
                        {
                            dictMainComposition.Add(context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                        }
                        else
                        {
                            dictMainComposition.Add(context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            dictMainComposition.Add(context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(mattress.compositionId).botSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                        }

                        if (context.MtrsCompositions.Find(mattress.compositionId).generalComposition != null)
                        {
                            dictMainComposition.Add(context.MtrsCompositions.Find(mattress.compositionId).generalComposition, new Dictionary<string, int>() { { Size, Numbers } });
                        }
                    }
                }

                if(context.Cuts.Find(mattress.cutId).topSideCompositionId == context.Cuts.Find(mattress.cutId).botSideCompositionId)
                {
                    switch(context.Cuts.Find(mattress.cutId).sectorName)
                    {
                        case "Ультразвук":
                            dictUltrCut = new Dictionary<string, Dictionary<string, int>>();
                            dictUltrCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            DistributeDictionary(dictUltrCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictUltrCut);
                            break;
                        case "V-16":
                            dictV16Cut = new Dictionary<string, Dictionary<string, int>>();
                            dictV16Cut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            DistributeDictionary(dictV16Cut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictV16Cut);
                            break;
                        case "Катерман":
                            dictKaterCut = new Dictionary<string, Dictionary<string, int>>();
                            dictKaterCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            DistributeDictionary(dictKaterCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictKaterCut);
                            break;
                        case "Не стегается":
                            dictNotStegCut = new Dictionary<string, Dictionary<string, int>>();
                            dictNotStegCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers * 2 } });
                            DistributeDictionary(dictNotStegCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictNotStegCut);
                            break;
                    }
                }
                else
                {
                    switch (context.Cuts.Find(mattress.cutId).sectorName)
                    {
                        case "Ультразвук":
                            dictUltrCut = new Dictionary<string, Dictionary<string, int>>();
                            dictUltrCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            dictUltrCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).botSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            DistributeDictionary(dictUltrCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictUltrCut);
                            break;
                        case "V-16":
                            dictV16Cut = new Dictionary<string, Dictionary<string, int>>();
                            dictV16Cut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            dictV16Cut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).botSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            DistributeDictionary(dictV16Cut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictV16Cut);
                            break;
                        case "Катерман":
                            dictKaterCut = new Dictionary<string, Dictionary<string, int>>();
                            dictKaterCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            dictKaterCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).botSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            DistributeDictionary(dictKaterCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictKaterCut);
                            break;
                        case "Не стегается":
                            dictNotStegCut = new Dictionary<string, Dictionary<string, int>>();
                            dictNotStegCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).topSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            dictNotStegCut.Add(context.CutCompositionSides.Find(context.Cuts.Find(mattress.cutId).botSideCompositionId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                            DistributeDictionary(dictNotStegCut);
                            dictMainComposition = DictionaryClear(dictMainComposition, dictNotStegCut);
                            break;
                    }
                }

                if (mattress.burletId != null)
                {
                    dictBurlet.Add(context.Burlets.Find(mattress.burletId).composition, new Dictionary<string, int>() { { Size, Numbers } });
                }

                dictMainComposition = DictionaryClear(dictMainComposition, dictPolyurethaneSheet);

            }
            // PgContext end.
        }
        //Konstr end.

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
                                {
                                    tempDictionary[tempStr] += Numbers;
                                }
                            }

                            tempStr = null;
                        }
                    }
                }
            }

            return tempDictionary;
        }

        private Dictionary<string,Dictionary<string,int>> UnitDictionaries(Dictionary<string, Dictionary<string, int>> currentDictionary, Dictionary<string, int> additionalDictionary, string size)
        {
            Dictionary<string, Dictionary<string, int>> tempDictionary = null;

            if (currentDictionary != null && additionalDictionary != null)
            {
                tempDictionary = currentDictionary;
                
                foreach(var item in additionalDictionary)
                {
                    if (!tempDictionary.ContainsKey(item.Key))
                        tempDictionary.Add(item.Key, new Dictionary<string, int>() { { size, item.Value } } );
                    else
                    {   
                        if (!tempDictionary[item.Key].ContainsKey(size))
                            tempDictionary[item.Key].Add(size, Numbers * 2);
                        else
                            tempDictionary[item.Key][size] += item.Value;
                    }
                }
            }

            return tempDictionary;
        }

        private Dictionary<string, Dictionary<string, int>> DictionaryClear(Dictionary<string, Dictionary<string, int>> currentDictionary, Dictionary<string, Dictionary<string, int>> dictionary)
        {
            Dictionary<string, Dictionary<string, int>> restulDict = new Dictionary<string, Dictionary<string, int>>();

            Dictionary<string, Dictionary<string, int>> tempDict = null;

            if (currentDictionary != null && dictionary != null)
            {
                tempDict = currentDictionary;

                string tempStr = null;
                List<string> tempList = new List<string>();

                string tempStr1 = null;
                List<string> newItems = new List<string>();

                foreach(var item in tempDict)
                {
                    foreach(char i in item.Key.ToArray())
                    {
                        if (i.ToString() != "/" && i.ToString() != ".")
                            tempStr += i;
                        else
                        {
                            tempList.Add(tempStr);

                            tempStr = null;
                        }
                    }

                    foreach (var itemIn in dictionary)
                    {
                        while (tempList.Contains(itemIn.Key))
                        {
                            tempList.Remove(itemIn.Key);
                        }
                    }

                    if(tempList.Count == 0)
                    {
                        tempDict.Remove(item.Key);
                        break;
                    }

                    foreach(string str in tempList)
                        tempStr1 += str + "/";

                    char[] arr = tempStr1.ToArray();
                    tempStr1 = null;

                    for (int i = 0; i <= arr.Length - 1; i++)
                    {
                        if (i != arr.Length - 1)
                            tempStr1 += arr[i];
                        else
                            tempStr1 += ".";
                    }

                    if(!restulDict.ContainsKey(tempStr1))
                        restulDict.Add(tempStr1, item.Value);

                    tempStr1 = null;
                }
            }

            return restulDict;
        }
        //

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

        private int GetMattressHeight(Mattresses mattress, string compositionStr)
        {
            int height = 0;

            using (PgContext context = new PgContext())
            {
                string tempStr = null;

                foreach (char i in compositionStr.ToArray())
                {
                    if (i.ToString() != "/" && i.ToString() != ".")
                        tempStr += i;
                    else
                    {
                        if (context.Materials.Find(tempStr)!=null && context.Materials.Find(tempStr).materialHeight != null)
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

        public override string ToString() => $"{Name}\nЗаказ: {OrderInfo} \nСтол:{TableName}\nРазмер: {Size}\nКоличество: {Numbers}\n***************************************************";

        //public override bool Equals(object obj)
        //{
        //    if (obj is MattressObject)
        //    {
        //        MattressObject tempObj = (MattressObject)obj;
        //        if (tempObj.Name.Equals(this.Name) & tempObj.Size.Equals(this.Size) & tempObj.OrderInfo.Equals(this.OrderInfo) & tempObj.TableName.Equals(this.TableName))
        //            return true;
        //    }
        //    return false;
        //}
        public override bool Equals(object obj) => obj.GetHashCode().Equals(this.GetHashCode()) ? true : false;

        public override int GetHashCode() => (OrderInfo.GetHashCode() * TableName.GetHashCode() * Name.GetHashCode() * Size.GetHashCode()) / 10000;

        #endregion
    }
}
