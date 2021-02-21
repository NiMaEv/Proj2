using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EntityModels;
using DictionaryExtensions;
using DictionaryExtensions.Special;

namespace ProjCity2
{
    public sealed class MattressObjectV2
    {
        public string OrderInfo { get; } //Attribut 
        public string TableName { get; } //Attribut
        public string Name { get; } //Attribute

        private int lenght;
        private int width;
        public string Size { get => $"{lenght}*{width}"; } //Attribute
        private string SizeForComponents { get; }
        private string SizeForBlocks { get; }
        public int Numbers { get; }
        public Mattresses Mattresses { get; }

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

        private Dictionary<string, Dictionary<string, int>> dictUltrCover;
        private Dictionary<string, Dictionary<string, int>> dictKaterCover;
        private Dictionary<string, Dictionary<string, int>> dictV16Cover;
        private Dictionary<string, Dictionary<string, int>> dictNotStegCover;
        #endregion

        public MattressObjectV2(string orderInfo, string tableName, Mattresses mattress, int lenght, int width, int numbers)
        {
            #region Initialize of Fields.
            OrderInfo = orderInfo;
            if (tableName != null)
                TableName = tableName;
            Name = mattress.mattressName;
            Mattresses = mattress;

            if (lenght < 1 | width < 1)
                throw new Exception("Недопустимый размер.");
            this.lenght = lenght;
            this.width = width;

            SizeForBlocks = SizeForComponents = Size;

            if (numbers > 0)
                Numbers = numbers;
            else
                throw new Exception("Количество: недопустимое значение.");

            #endregion

            using (PgContext context = new PgContext())
            {
                MtrsCompositions mainComposition = context.MtrsCompositions.Find(mattress.compositionId);
                if (mainComposition.topSideCompositionId == null & mainComposition.botSideCompositionId == null & mainComposition.generalComposition == null)
                    throw new Exception($"Неверный формат записи.\nMtrsCompositions/compositionId({mainComposition.compositionId}).");
                dictMainComposition = new Dictionary<string, Dictionary<string, int>>();

                // ???
                string topperComposition = null;
                string topperCompositionOfTopSideCut = null;
                string topperCompositionOfBotSideCut = null;
                string topperCompositionOfCover = null;
                if (mattress.topperId != null)
                {
                    Toppers topper = context.Toppers.Find(mattress.topperId);    
                    if (topper.compositionId != null)
                        topperComposition = context.TopperCompositions.Find(topper.compositionId).composition;
                    if (topper.cutId != null)
                    {
                        TopperCuts topperCut = context.TopperCuts.Find(topper.cutId);
                        if (topperCut.topSideCompositionId == topperCut.botSideCompositionId)
                            topperCompositionOfTopSideCut = topperCompositionOfBotSideCut = context.TopperCutCompositiont.Find(topperCut.topSideCompositionId).composition;
                        else
                        {
                            topperCompositionOfTopSideCut = context.TopperCutCompositiont.Find(topperCut.topSideCompositionId).composition;
                            topperCompositionOfBotSideCut = context.TopperCutCompositiont.Find(topperCut.botSideCompositionId).composition;
                        }
                        if (topperCut.cutCase != null)
                            topperCompositionOfCover = topperCut.cutCase;
                    }
                }

                #region Inserting in dictPolyurethaneForPerimetr.
                if (mattress.perimetrId != null)
                {
                    Perimetrs perimetr = context.Perimetrs.Find(mattress.perimetrId);
                    if (perimetr.reinforcmentBlockMaterialName != null | perimetr.reinforcmentMattressMaterialName != null)
                    {
                        string xLenght = $"{lenght}";
                        string yLenght = $"{width - (perimetr.reinforcmentMaterialWidth * 2)}";
                        dictPolyurethaneForPerimetr = new Dictionary<string, Dictionary<string, int>>();
                        if (perimetr.reinforcmentBlockMaterialName != null)
                        {
                            SizeForBlocks = $"{lenght - perimetr.reinforcmentMaterialWidth * 2}*{width - perimetr.reinforcmentMaterialWidth * 2}";

                            Blocks mainBlock = context.Blocks.Find(mainComposition.blockId);
                            Dictionary<string, int> tempDict = new Dictionary<string, int>() { { perimetr.reinforcmentBlockMaterialName +
                                    " h=" + mainBlock.blockHeight / 10, Numbers * 2 } };
                            dictPolyurethaneForPerimetr.Add(xLenght, tempDict);
                            dictPolyurethaneForPerimetr.Add(yLenght, tempDict);

                            if (mainComposition.additionalBlockId != null)
                            {
                                Blocks additionalBlock = context.Blocks.Find(mainComposition.additionalBlockId);
                                dictPolyurethaneForPerimetr[xLenght].Add(perimetr.reinforcmentBlockMaterialName +
                                    " h=" + additionalBlock.blockHeight / 10, Numbers * 2);
                                dictPolyurethaneForPerimetr[yLenght].Add(perimetr.reinforcmentBlockMaterialName +
                                    " h=" + additionalBlock.blockHeight / 10, Numbers * 2);
                            }
                        } 
                        if (perimetr.reinforcmentMattressMaterialName != null)
                        {
                            SizeForBlocks = SizeForComponents = $"{lenght - perimetr.reinforcmentMaterialWidth * 2}*{width - perimetr.reinforcmentMaterialWidth * 2}";

                            Dictionary<string, int> tempDict = new Dictionary<string, int>() { { perimetr.reinforcmentMattressMaterialName +
                                    " h=" + GetMattressHeight(mattress) / 10, Numbers * 2} };
                            dictPolyurethaneForPerimetr.Add(xLenght, tempDict);
                            dictPolyurethaneForPerimetr.Add(yLenght, tempDict);
                        }
                    }
                }
                #endregion

                #region Inserting in dictPolyurethaneSheet.
                string mainCompositionStr = null;
                if (mainComposition.generalComposition != null)
                    mainCompositionStr += context.MtrsCompositions.Find(mainComposition.compositionId).generalComposition;
                if (mainComposition.topSideCompositionId != null)
                    mainCompositionStr += context.MtrsCompositionSides.Find(mainComposition.topSideCompositionId).composition;
                if (mainComposition.botSideCompositionId != null)
                    mainCompositionStr += context.MtrsCompositionSides.Find(mainComposition.botSideCompositionId).composition;
                if (mainCompositionStr.GetDictionary("ППУ", Numbers).Count != 0)
                {
                    dictPolyurethaneSheet = new Dictionary<string, Dictionary<string, int>>();
                    dictPolyurethaneSheet.Unite(SizeForComponents, mainCompositionStr.GetDictionary("ППУ", Numbers));
                    if (topperComposition != null)// ???
                        dictPolyurethaneSheet.Unite(Size, topperComposition.GetDictionary("ППУ", Numbers));
                }
                #endregion

                #region Inserting in dictMainComposition.
                if (mainComposition.topSideCompositionId != null & mainComposition.botSideCompositionId != null)
                {
                    MtrsCompositionSides compositionOfTopSide = context.MtrsCompositionSides.Find(mainComposition.topSideCompositionId);
                    if(mainComposition.topSideCompositionId == mainComposition.botSideCompositionId)
                    {
                        dictMainComposition.Add(compositionOfTopSide.composition, new Dictionary<string, int> { { SizeForComponents, Numbers * 2 } });
                    }
                    else
                    {
                        MtrsCompositionSides compositionOfBotSide = context.MtrsCompositionSides.Find(mainComposition.botSideCompositionId);
                        dictMainComposition.Add(compositionOfTopSide.composition, new Dictionary<string, int> { { SizeForComponents, Numbers } });
                        dictMainComposition.Add(compositionOfBotSide.composition, new Dictionary<string, int> { { SizeForComponents, Numbers } });
                    }
                }
                if (mainComposition.generalComposition != null)
                    dictMainComposition.Add(mainComposition.generalComposition, new Dictionary<string, int> { { SizeForComponents, Numbers } });
                if (topperComposition != null) // ???
                    dictMainComposition.Add(topperComposition, new Dictionary<string, int> { { Size, Numbers } });
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

                // composition + (MattressName) 
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
                string compositionOfCover = null;
                if (mainCut.cutCase != null)
                    compositionOfCover = mainCut.cutCase;
                
                switch (mainCut.sectorName)
                {
                    case "Ультразвук":
                        dictUltrCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (compositionOfTopSideCut == compositionOfBotSideCut)
                                dictUltrCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers * 2);
                            else
                            {
                                dictUltrCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers);
                                dictUltrCut.InsertCut(compositionOfBotSideCut.composition, Size, numbers);
                            }
                        }
                        dictUltrCut.InsertComponents(mainCompositionStr, Size, Numbers);
                        if (topperCompositionOfTopSideCut != null & topperCompositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (topperCompositionOfTopSideCut == topperCompositionOfBotSideCut)
                                dictUltrCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers * 2);
                            else
                            {
                                dictUltrCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers);
                                dictUltrCut.InsertCut(topperCompositionOfBotSideCut, Size, Numbers);
                            }
                        }
                        if (topperComposition != null)
                            dictUltrCut.InsertComponents(topperComposition, Size, Numbers);
                        if (compositionOfCover != null | topperCompositionOfCover != null) // compositionOfCover + $" ({Name})"
                        {
                            dictUltrCover = new Dictionary<string, Dictionary<string, int>>();
                            if (compositionOfCover != null)
                                dictUltrCover.InsertCut(compositionOfCover, Size, Numbers);
                            if (topperCompositionOfCover != null)
                                dictUltrCover.InsertCut(topperCompositionOfCover, Size, Numbers);
                        }
                        dictMainComposition.RemoveMatches(dictUltrCut, Size);
                        break;
                    case "V-16":
                        dictV16Cut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (compositionOfTopSideCut == compositionOfBotSideCut)
                                dictV16Cut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers * 2);
                            else
                            {
                                dictV16Cut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers);
                                dictV16Cut.InsertCut(compositionOfBotSideCut.composition, Size, numbers);
                            }
                        }
                        dictV16Cut.InsertComponents(mainCompositionStr, Size, Numbers);
                        if (topperCompositionOfTopSideCut != null & topperCompositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (topperCompositionOfTopSideCut == topperCompositionOfBotSideCut)
                                dictV16Cut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers * 2);
                            else
                            {
                                dictV16Cut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers);
                                dictV16Cut.InsertCut(topperCompositionOfBotSideCut, Size, Numbers);
                            }
                        }
                        if (topperComposition != null)
                            dictV16Cut.InsertComponents(topperComposition, Size, Numbers);
                        if (compositionOfCover != null | topperCompositionOfCover != null) // compositionOfCover + $" ({Name})"
                        {
                            dictV16Cover = new Dictionary<string, Dictionary<string, int>>();
                            if (compositionOfCover != null)
                                dictV16Cover.InsertCut(compositionOfCover, Size, Numbers);
                            if (topperCompositionOfCover != null)
                                dictV16Cover.InsertCut(topperCompositionOfCover, Size, Numbers);
                        }
                        dictMainComposition.RemoveMatches(dictV16Cut, Size);
                        break;
                    case "Катерман":
                        dictKaterCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (compositionOfTopSideCut == compositionOfBotSideCut)
                                dictKaterCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers * 2);
                            else
                            {
                                dictKaterCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers);
                                dictKaterCut.InsertCut(compositionOfBotSideCut.composition, Size, numbers);
                            }
                        }
                        dictKaterCut.InsertComponents(mainCompositionStr, Size, Numbers);
                        if (topperCompositionOfTopSideCut != null & topperCompositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (topperCompositionOfTopSideCut == topperCompositionOfBotSideCut)
                                dictKaterCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers * 2);
                            else
                            {
                                dictKaterCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers);
                                dictKaterCut.InsertCut(topperCompositionOfBotSideCut, Size, Numbers);
                            }
                        }
                        if (topperComposition != null)
                            dictKaterCut.InsertComponents(topperComposition, Size, Numbers);
                        if (compositionOfCover != null | topperCompositionOfCover != null) // compositionOfCover + $" ({Name})"
                        {
                            dictKaterCover = new Dictionary<string, Dictionary<string, int>>();
                            if (compositionOfCover != null)
                                dictKaterCover.InsertCut(compositionOfCover, Size, Numbers);
                            if (topperCompositionOfCover != null)
                                dictKaterCover.InsertCut(topperCompositionOfCover, Size, Numbers);
                        }
                        dictMainComposition.RemoveMatches(dictKaterCut, Size);
                        break;
                    case "Не стегается":
                        dictNotStegCut = new Dictionary<string, Dictionary<string, int>>();
                        if (compositionOfTopSideCut != null & compositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (compositionOfTopSideCut == compositionOfBotSideCut)
                                dictNotStegCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers * 2);
                            else
                            {
                                dictNotStegCut.InsertCut(compositionOfTopSideCut.composition, Size, Numbers);
                                dictNotStegCut.InsertCut(compositionOfBotSideCut.composition, Size, numbers);
                            }
                        }
                        dictNotStegCut.InsertComponents(mainCompositionStr, Size, Numbers);
                        if (topperCompositionOfTopSideCut != null & topperCompositionOfBotSideCut != null) //Composition + $" ({Name})"
                        {
                            if (topperCompositionOfTopSideCut == topperCompositionOfBotSideCut)
                                dictNotStegCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers * 2);
                            else
                            {
                                dictNotStegCut.InsertCut(topperCompositionOfTopSideCut, Size, Numbers);
                                dictNotStegCut.InsertCut(topperCompositionOfBotSideCut, Size, Numbers);
                            }
                        }
                        if (topperComposition != null)
                            dictNotStegCut.InsertComponents(topperComposition, Size, Numbers);
                        if (compositionOfCover != null | topperCompositionOfCover != null) // compositionOfCover + $" ({Name})"
                        {
                            dictNotStegCover = new Dictionary<string, Dictionary<string, int>>();
                            if (compositionOfCover != null)
                                dictNotStegCover.InsertCut(compositionOfCover, Size, Numbers);
                            if (topperCompositionOfCover != null)
                                dictNotStegCover.InsertCut(topperCompositionOfCover, Size, Numbers);
                        }
                        dictMainComposition.RemoveMatches(dictNotStegCut, Size);
                        break;
                    default:
                        throw new Exception($"Неверный формат записи.\nCuts/sectorName. cutId({mainCut.cutId})");
                }
                #endregion //

                // + perimetr composition. + (MattressName)
                #region Inserting in dictBurlet.
                if (mattress.burletId != null)
                {
                    Burlets burlet = context.Burlets.Find(mattress.burletId);   
                    dictBurlet = new Dictionary<string, Dictionary<string, int>>();
                    dictBurlet.Add(burlet.composition, new Dictionary<string, int> { { Size, Numbers } }); // burlet.composition + $" ({Name})"
                }
                #endregion

                if (dictPolyurethaneSheet != null)
                    dictMainComposition.RemoveMatches(dictPolyurethaneSheet, SizeForComponents);
            }
        }

        #region Methods.
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
                    height += context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).blockId).blockHeight;
                if (context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId != null)
                    height += context.Blocks.Find(context.MtrsCompositions.Find(mattress.compositionId).additionalBlockId).blockHeight;
            }

            return height;
        }

        #region Methods of Gets Dictionaryies.
        public Dictionary<string, Dictionary<string, int>> GetPolyurethaneSheetsDictionary() => dictPolyurethaneSheet.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetPolyurethaneForPerimetrDictionary() => dictPolyurethaneForPerimetr.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetMainCompositionDictionary() => dictMainComposition.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetBlocksDictionary() => dictBlocks.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetUltrCutDictionary() => dictUltrCut.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetV16CutDictionary() => dictV16Cut.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetKaterCutDictionary() => dictKaterCut.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetNotStegCutDictionary() => dictNotStegCut.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetBurletDictionary() => dictBurlet.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetUltrCoverDictionary() => dictUltrCover.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetV16CoverDictionary() => dictV16Cover.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetKaterCoverDictionary() => dictKaterCover.CopyDictionary();
        public Dictionary<string, Dictionary<string, int>> GetNotStegCoverDictionary() => dictNotStegCover.CopyDictionary();
        #region Methods of Gets Counts.
        public int GetCountOfPolyurethaneSheetDictionary() => dictPolyurethaneSheet == null ? 0 : dictPolyurethaneSheet.Count;
        public int GetCountOfPolyurethaneForPerimetrDictionary() => dictPolyurethaneForPerimetr == null ? 0 : dictPolyurethaneForPerimetr.Count;
        public int GetCountOfMainCompositionDictionary() => dictMainComposition == null ? 0 : dictMainComposition.Count;
        public int GetCountOfBlockDictionary() => dictBlocks == null ? 0 : dictBlocks.Count;
        public int GetCountOfUltrCutDictionary() => dictUltrCut == null ? 0 : dictUltrCut.Count;
        public int GetCountOfV16CutDictionary() => dictV16Cut == null ? 0 : dictV16Cut.Count;
        public int GetCountOfKaterCutDictionary() => dictKaterCut == null ? 0 : dictKaterCut.Count;
        public int GetCountOfNotStegCutDictionry() => dictNotStegCut == null ? 0 : dictNotStegCut.Count;
        public int GetCountOfBurletDictionary() => dictBurlet == null ? 0 : dictBurlet.Count;
        public int GetCountOfUltrCoverDictionary() => dictUltrCover == null ? 0 : dictUltrCover.Count;
        public int GetCountOfV16CoverDictionary() => dictV16Cover == null ? 0 : dictV16Cover.Count;
        public int GetCountOfKaterCoverDictionary() => dictKaterCover == null ? 0 : dictKaterCover.Count;
        public int GetCountOfNotStegCoverDictionary() => dictNotStegCover == null ? 0 : dictNotStegCover.Count;
        #endregion
        #endregion

        public int GetLenght() => lenght;
        public int GetWidth() => width;

        #region Base Methods.
        public override string ToString() => TableName != null ? $"{Name}\nЗаказ: {OrderInfo} \nСтол:{TableName}\nРазмер: {Size}\nКоличество: {Numbers}\n***************************************************"
            : $"{Name}\nЗаказ: {OrderInfo}\nРазмер: {Size}\nКоличество: {Numbers}\n***************************************************";

        public override bool Equals(object obj) => obj == null ? false : GetHashCode() == obj.GetHashCode();

        public static bool operator ==(MattressObjectV2 mtrsObj1, MattressObjectV2 mtrsObj2) => mtrsObj1.Equals(mtrsObj2);
        public static bool operator !=(MattressObjectV2 mtrsObj1, MattressObjectV2 mtrsObj2) => !mtrsObj1.Equals(mtrsObj2);

        public override int GetHashCode() => TableName != null ? OrderInfo.GetHashCode() * TableName.GetHashCode() * Name.GetHashCode() * Size.GetHashCode()
            : OrderInfo.GetHashCode() * Name.GetHashCode() * Size.GetHashCode();
        #endregion
        #endregion
    }
}