using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace EntityModels
{
    [Table("public.Toppers")]
    public partial class Toppers : IEntityExtensions<Toppers>, IEntityAdvanceExnensions, ICopying<Toppers>, IDuplication<Toppers>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Toppers()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int topperId { get; set; }

        public int? compositionId { get; set; }

        public int? cutId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        public virtual TopperCompositions TopperCompositions { get; set; }

        public virtual TopperCuts TopperCuts { get; set; }

        public bool CompareTo(Toppers other)
        {
            if (compositionId != other.compositionId) return false;
            if (cutId != other.cutId) return false;
            return true;
        }

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            using (PgContext context = new PgContext())
                foreach (var item in context.Mattresses)
                    if (item.topperId != null)
                        if (item.topperId == topperId) list.Add(item);
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => topperId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            topperId = value;
        }

        public Toppers GetCopy() => new Toppers { topperId = this.topperId, compositionId = this.compositionId, cutId = this.cutId };

        public void Duplicate(Toppers obj)
        {
            topperId = obj.topperId;
            compositionId = obj.compositionId;
            cutId = obj.cutId;
        }
    }
}
