using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.MtrsCompositions")]
    public partial class MtrsCompositions : IEntityExtensions<MtrsCompositions>, IEntityAdvanceExnensions, ICopying<MtrsCompositions>, IDuplication<MtrsCompositions>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MtrsCompositions()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int compositionId { get; set; }

        //public int? topSideCompositionId { get; set; }

        //public int? botSideCompositionId { get; set; }

        [StringLength(700)]
        public string generalComposition { get; set; }

        public int? blockId { get; set; }

        public int? additionalBlockId { get; set; }

        public virtual Blocks Blocks { get; set; }

        public virtual Blocks Blocks1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        //public virtual MtrsCompositionSides MtrsCompositionSides { get; set; }

        //public virtual MtrsCompositionSides MtrsCompositionSides1 { get; set; }

        public bool CompareTo(MtrsCompositions other)
        {
            if (generalComposition != other.generalComposition) return false;
            if (blockId != other.blockId) return false;
            if (additionalBlockId != other.additionalBlockId) return false;
            return true;
        }

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            using (PgContext context = new PgContext())
                foreach (var item in context.Mattresses)
                    if (item.compositionId == compositionId) list.Add(item);
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => compositionId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            compositionId = value;
        }

        public MtrsCompositions GetCopy() => new MtrsCompositions { compositionId = this.compositionId, generalComposition = this.generalComposition,
            blockId = this.blockId, additionalBlockId = this.additionalBlockId};

        public void Duplicate(MtrsCompositions obj)
        {
            compositionId = obj.compositionId;
            generalComposition = obj.generalComposition;
            blockId = obj.blockId;
            additionalBlockId = obj.additionalBlockId;
        }
    }
}
