using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.TopperCutCompositiont")]
    public partial class TopperCutCompositiont : IEntityExtensions<TopperCutCompositiont>, IEntityAdvanceExnensions, ICopying<TopperCutCompositiont>, IDuplication<TopperCutCompositiont>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int compositionId { get; set; }

        [StringLength(500)]
        public string composition { get; set; }

        public bool CompareTo(TopperCutCompositiont other) => composition == other.composition;

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            List<TopperCuts> innerList = new List<TopperCuts>();
            List<Toppers> innerTList = new List<Toppers>();
            using (PgContext context = new PgContext())
            {
                foreach (var item in context.TopperCuts)
                    if (item.topSideCompositionId != null & item.botSideCompositionId != null)
                        if (item.topSideCompositionId == compositionId | item.botSideCompositionId == compositionId) innerList.Add(item);
                if (innerList.Count == 0) return false;
                foreach (var item in context.Toppers)
                    if (item.cutId != null)
                        if (innerList.Find(i => i.cutId == item.cutId) != null) innerTList.Add(item);
                if (innerTList.Count == 0) return false;
                foreach (var item in context.Mattresses)
                    if (item.topperId != null)
                        if (innerTList.Find(i => i.topperId == item.topperId) != null) list.Add(item);
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

        public TopperCutCompositiont GetCopy() => new TopperCutCompositiont { compositionId = this.compositionId, composition = this.composition };

        public void Duplicate(TopperCutCompositiont obj)
        {
            compositionId = obj.compositionId;
            composition = obj.composition;
        }
    }
}
