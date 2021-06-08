using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.CutCompositionSides")]
    public partial class CutCompositionSides : IEntityExtensions<CutCompositionSides>, IEntityAdvanceExnensions, ICopying<CutCompositionSides>, IDuplication<CutCompositionSides>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CutCompositionSides()
        {
            Cuts = new HashSet<Cuts>();
            Cuts1 = new HashSet<Cuts>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int compositionId { get; set; }

        [Required]
        [StringLength(500)]
        public string composition { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuts> Cuts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuts> Cuts1 { get; set; }

        public bool CompareTo(CutCompositionSides other) => composition == other.composition & description == other.description;

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            List<Cuts> innerList = new List<Cuts>();
            using (PgContext context = new PgContext())
            {
                foreach (var item in context.Cuts)
                    if (item.topSideCompositionId != null & item.botSideCompositionId != null)
                        if (item.topSideCompositionId == compositionId | item.botSideCompositionId == compositionId) innerList.Add(item);
                if (innerList.Count == 0) return false;
                //foreach (var item in innerList)
                //    foreach (var innerItem in context.Mattresses)
                //        if (innerItem.cutId == item.cutId) list.Add(innerItem);
                foreach (var item in context.Mattresses)
                    if (innerList.Find(i => i.cutId == item.cutId) != null) list.Add(item);
            }
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => compositionId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            compositionId = value;
        }

        public CutCompositionSides GetCopy() => new CutCompositionSides { compositionId = this.compositionId, composition = this.composition, description = this.description };

        public void Duplicate(CutCompositionSides obj)
        {
            compositionId = obj.compositionId;
            composition = obj.composition;
            description = obj.description;
        }
    }
}
