using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Cuts")]
    public partial class Cuts
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

        public virtual CutCompositionSides CutCompositionSides { get; set; }

        public virtual CutCompositionSides CutCompositionSides1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }
    }
}
