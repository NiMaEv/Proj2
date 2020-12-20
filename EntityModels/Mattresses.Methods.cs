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
            string statusStr = null;

            using (PgContext context = new PgContext())
            {
                statusStr += $"{mattressName}\n";
                statusStr += $"Линия: {context.Series.Find(seriesId).seriesName}\n";

                MtrsCompositions composition = context.MtrsCompositions.Find(compositionId);
                statusStr += "Состав: ";
                if (composition.blockId != null)
                    statusStr += $"Блок: {context.Blocks.Find(composition.blockId).blockName}";
                if (composition.additionalBlockId != null)
                    statusStr += $", {context.Blocks.Find(composition.additionalBlockId).blockName}";
                if (composition.generalComposition != null)
                    statusStr += $"\n{composition.generalComposition}";
                if (composition.topSideCompositionId != null & composition.botSideCompositionId != null)
                {
                    if (composition.topSideCompositionId == composition.botSideCompositionId)
                        statusStr += $"\n2 стороны: {context.MtrsCompositionSides.Find(composition.topSideCompositionId).composition}";
                    else
                        statusStr += $"\n1 сторона: {context.MtrsCompositionSides.Find(composition.topSideCompositionId).composition}" +
                            $"\n2 сторона: {context.MtrsCompositionSides.Find(composition.botSideCompositionId).composition}";
                }

                Cuts cut = context.Cuts.Find(cutId);
                statusStr += "\nКрой: ";
                if (cut.cutCase != null)
                    statusStr += $"Чехол: {cut.cutCase}";
                if (cut.topSideCompositionId != null & cut.botSideCompositionId != null)
                {
                    if (cut.topSideCompositionId == cut.botSideCompositionId)
                        statusStr += $"2 стороны: {context.CutCompositionSides.Find(cut.topSideCompositionId).composition}";
                    else
                        statusStr += $"1 сторона: {context.CutCompositionSides.Find(cut.topSideCompositionId).composition}" +
                            $"\n2 сторона: {context.CutCompositionSides.Find(cut.botSideCompositionId).composition}";
                }

                if (burletId != null)
                {
                    Burlets burlet = context.Burlets.Find(burletId);
                    statusStr += $"\nБурлет: {burlet.composition}";
                    if (burlet.description != null)
                        statusStr += $" ({burlet.description})";
                }

                if (perimetrId != null)
                {
                    statusStr += "\nПериметр: ";
                    Perimetrs perimetr = context.Perimetrs.Find(perimetrId);
                    statusStr += perimetr.reinforcmentMattressMaterialName;
                    statusStr += perimetr.reinforcmentBlockMaterialName;
                    if (perimetr.composition != null)
                        statusStr += $"/{perimetr.composition}";
                }
            }
            statusStr += "\n*********************************************************************************************************************************************************************";

            return statusStr;
        }
    }
}
