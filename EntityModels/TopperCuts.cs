using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.TopperCuts")]
    public partial class TopperCuts : IEntityExtensions<TopperCuts>, IEntityAdvanceExnensions, ICopying<TopperCuts>, IDuplication<TopperCuts>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TopperCuts()
        {
            Toppers = new HashSet<Toppers>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cutId { get; set; }

        public int? topSideCompositionId { get; set; }

        public int? botSideCompositionId { get; set; }

        [StringLength(300)]
        public string cutCase { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Toppers> Toppers { get; set; }

        public bool CompareTo(TopperCuts other)
        {
            if (topSideCompositionId != other.topSideCompositionId) return false;
            if (botSideCompositionId != other.botSideCompositionId) return false;
            if (cutCase != other.cutCase) return false;
            return true;
        }

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            List<Toppers> innerList = new List<Toppers>();
            using (PgContext context = new PgContext())
            {
                foreach (var item in context.Toppers)
                    if (item.cutId != null)
                        if (item.cutId == cutId) innerList.Add(item);
                if (innerList.Count == 0) return false;
                foreach (var item in context.Mattresses)
                    if (item.topperId != null)
                        if (innerList.Find(i => i.topperId == item.topperId) != null) list.Add(item);
            }
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => cutId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            cutId = value;
        }

        public TopperCuts GetCopy() => new TopperCuts { cutId = this.cutId, topSideCompositionId = this.topSideCompositionId, botSideCompositionId = this.botSideCompositionId, cutCase = this.cutCase };
    
        public void Duplicate(TopperCuts obj)
        {
            cutId = obj.cutId;
            topSideCompositionId = obj.topSideCompositionId;
            botSideCompositionId = obj.botSideCompositionId;
            cutCase = obj.cutCase;
        }
    }
}
