using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Cuts")]
    public partial class Cuts : IEntityExtensions<Cuts>, IEntityAdvanceExnensions, ICopying<Cuts>, IDuplication<Cuts>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cuts()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cutId { get; set; }

        public int? topSideCompositionId { get; set; }

        public int? botSideCompositionId { get; set; }

        [StringLength(300)]
        public string cutCase { get; set; }

        [StringLength(500)]
        public string desciption { get; set; }

        [StringLength(500)]
        public string sectorName { get; set; }

        public virtual CutCompositionSides CutCompositionSides { get; set; }

        public virtual CutCompositionSides CutCompositionSides1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        public bool CompareTo(Cuts other)
        {
            if (topSideCompositionId != other.topSideCompositionId) return false;
            if (botSideCompositionId != other.botSideCompositionId) return false;
            if (cutCase != other.cutCase) return false;
            if (desciption != other.desciption) return false;
            if (sectorName != other.sectorName) return false;
            return true;
        }

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            using (PgContext context = new PgContext())
                foreach (var item in context.Mattresses)
                    if (item.cutId == cutId) list.Add(item);
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => cutId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            cutId = value;
        }

        public Cuts GetCopy() => new Cuts { cutId = this.cutId, topSideCompositionId = this.topSideCompositionId, botSideCompositionId = this.botSideCompositionId,
            cutCase = this.cutCase, desciption = this.desciption, sectorName = this.sectorName};

        public void Duplicate(Cuts obj)
        {
            cutId = obj.cutId;
            topSideCompositionId = obj.topSideCompositionId;
            botSideCompositionId = obj.botSideCompositionId;
            cutCase = obj.cutCase;
            desciption = obj.desciption;
            sectorName = obj.sectorName;
        }
    }
}
