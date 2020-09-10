using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.MtrsCompositions")]
    public partial class MtrsCompositions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MtrsCompositions()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int compositionId { get; set; }

        public int? topSideCompositionId { get; set; }

        public int? botSideCompositionId { get; set; }

        [StringLength(700)]
        public string generalComposition { get; set; }

        public int? blockId { get; set; }

        public int? additionalBlockId { get; set; }

        public virtual Blocks Blocks { get; set; }

        public virtual Blocks Blocks1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        public virtual MtrsCompositionSides MtrsCompositionSides { get; set; }

        public virtual MtrsCompositionSides MtrsCompositionSides1 { get; set; }
    }
}
