using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Perimetrs")]
    public partial class Perimetrs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Perimetrs()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int perimetrId { get; set; }

        [StringLength(500)]
        public string reinforcmentMattressMaterialName { get; set; }

        [StringLength(500)]
        public string reinforcmentBlockMaterialName { get; set; }

        public int? reinforcmentMaterialWidth { get; set; }

        [StringLength(500)]
        public string composition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }
    }
}
