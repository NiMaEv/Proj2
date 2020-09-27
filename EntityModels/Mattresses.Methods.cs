using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels
{
    public partial class Mattresses
    {
        public override string ToString()
        {
            string tempStr = null;

            using (PgContext context = new PgContext())
            {
                tempStr += $"{mattressName}\n";
                tempStr += $"Линия: {context.Series.Find(seriesId).seriesName}\n";

                if (context.MtrsCompositions.Find(compositionId).blockId != null)
                {
                    tempStr += $"Состав: Блок {context.Blocks.Find(context.MtrsCompositions.Find(compositionId).blockId).blockName}";
                    if (context.MtrsCompositions.Find(compositionId).additionalBlockId != null)
                        tempStr += $" + {context.Blocks.Find(context.MtrsCompositions.Find(compositionId).additionalBlockId).blockName}";
                    tempStr += "\n";
                }
                else
                    tempStr += "Состав: ";

                if (context.MtrsCompositions.Find(compositionId).topSideCompositionId != null && context.MtrsCompositions.Find(compositionId).botSideCompositionId != null)
                {
                    if (context.MtrsCompositions.Find(compositionId).topSideCompositionId == context.MtrsCompositions.Find(compositionId).botSideCompositionId)
                    {
                        if (context.MtrsCompositions.Find(compositionId).generalComposition != null)
                            tempStr += $"Блок: {context.MtrsCompositions.Find(compositionId).generalComposition}\n";
                        tempStr += $"2 стороны: {context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(compositionId).topSideCompositionId).composition}\n";
                    }
                    else
                    {
                        tempStr += $"1 сторона: {context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(compositionId).topSideCompositionId).composition}\n";
                        if (context.MtrsCompositions.Find(compositionId).generalComposition != null)
                            tempStr += $"{context.MtrsCompositions.Find(compositionId).generalComposition}\n";
                        tempStr += $"2 сторона: {context.MtrsCompositionSides.Find(context.MtrsCompositions.Find(compositionId).botSideCompositionId).composition}\n";
                    }
                }
                else
                    tempStr += $"{context.MtrsCompositions.Find(compositionId).generalComposition}\n";

                if (context.Cuts.Find(cutId).desciption != null)
                    tempStr += $"Крой({context.Cuts.Find(cutId).desciption}): ";
                else
                    tempStr += "Крой: ";

                if (context.Cuts.Find(cutId).cutCase != null)
                    tempStr += $"1.{context.Cuts.Find(cutId).cutCase} 2.";

                if (context.Cuts.Find(cutId).topSideCompositionId == context.Cuts.Find(cutId).botSideCompositionId)
                    tempStr += $"2 стороны: {context.CutCompositionSides.Find(context.Cuts.Find(cutId).topSideCompositionId).composition}\n";
                else
                    tempStr += $"1 сторона: {context.CutCompositionSides.Find(context.Cuts.Find(cutId).topSideCompositionId).composition}\n2 сторона: {context.CutCompositionSides.Find(context.Cuts.Find(cutId).botSideCompositionId).composition}\n";

                if (burletId != null)
                {
                    tempStr += $"Бурлет: {context.Burlets.Find(burletId).composition} ";
                    if (context.Burlets.Find(burletId).description != null)
                        tempStr += $"({context.Burlets.Find(burletId).description})\n";
                    else
                        tempStr += "\n";
                }

                if (perimetrId != null)
                {
                    tempStr += "Периметр: ";
                    if (context.Perimetrs.Find(perimetrId).reinforcmentMattressMaterialName != null)
                        tempStr += $"{context.Perimetrs.Find(perimetrId).reinforcmentMattressMaterialName}";
                    if (context.Perimetrs.Find(perimetrId).reinforcmentBlockMaterialName != null)
                        tempStr += $"{context.Perimetrs.Find(perimetrId).reinforcmentBlockMaterialName}";
                    tempStr += $"/{context.Perimetrs.Find(perimetrId).composition}";
                }
                tempStr += $"\n*********************************************************************************************************************************************************************";

                return tempStr;
            }
        }
    }
}
