using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.MtrsCompositionSides")]
    public partial class MtrsCompositionSides
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MtrsCompositionSides()
        {
            MtrsCompositions = new HashSet<MtrsCompositions>();
            MtrsCompositions1 = new HashSet<MtrsCompositions>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int compositionId { get; set; }

        [Required]
        [StringLength(500)]
        public string composition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MtrsCompositions> MtrsCompositions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MtrsCompositions> MtrsCompositions1 { get; set; }

        public bool CompareTo(MtrsCompositionSides mcsObj) => (mcsObj.compositionId == this.compositionId & mcsObj.composition.Equals(this.composition)) ? true : false;
    }
}
