using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Blocks")]
    public partial class Blocks : IEntityExtensions<Blocks>, ICopying<Blocks>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Blocks()
        {
            MtrsCompositions = new HashSet<MtrsCompositions>();
            MtrsCompositions1 = new HashSet<MtrsCompositions>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int blockId { get; set; }

        [Required]
        [StringLength(300)]
        public string blockName { get; set; }

        public int blockHeight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MtrsCompositions> MtrsCompositions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MtrsCompositions> MtrsCompositions1 { get; set; }

        public bool CompareTo(Blocks other) => blockName == other.blockName & blockHeight == other.blockHeight;

        public Blocks GetCopy() => new Blocks { blockId = this.blockId, blockName = this.blockName, blockHeight = this.blockHeight };

        public int GetId() => blockId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            blockId = value;
        }

        public override string ToString() => blockName;

        public override int GetHashCode() => blockId * blockName.GetHashCode() * blockHeight ^ 2 * 9341;

        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
    }
}
