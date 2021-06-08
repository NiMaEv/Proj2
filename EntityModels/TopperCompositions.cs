using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.TopperCompositions")]
    public partial class TopperCompositions : IEntityExtensions<TopperCompositions>, IEntityAdvanceExnensions, ICopying<TopperCompositions>, IDuplication<TopperCompositions>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopperCompositions()
        {
            Toppers = new HashSet<Toppers>();
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
        public virtual ICollection<Toppers> Toppers { get; set; }

        public bool CompareTo(TopperCompositions other) => composition == other.composition & description == other.description;

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            List<Toppers> innerList = new List<Toppers>();
            using (PgContext context = new PgContext())
            {
                foreach (var item in context.Toppers)
                    if (item.compositionId != null)
                        if (item.compositionId == compositionId) innerList.Add(item);
                if (innerList.Count == 0) return false;
                foreach (var item in context.Mattresses)
                    if (item.topperId != null)
                        if (innerList.Find(i => i.topperId == item.topperId) != null) list.Add(item);
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

        public TopperCompositions GetCopy() => new TopperCompositions { compositionId = this.compositionId, composition = this.composition };

        public void Duplicate(TopperCompositions obj)
        {
            compositionId = obj.compositionId;
            composition = obj.composition;
            description = obj.description;
        }
    }
}
