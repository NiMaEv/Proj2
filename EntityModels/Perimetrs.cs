using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Perimetrs")]
    public partial class Perimetrs : IEntityExtensions<Perimetrs>, IEntityAdvanceExnensions, ICopying<Perimetrs>, IDuplication<Perimetrs>
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

        public bool CompareTo(Perimetrs other)
        {
            if (reinforcmentMattressMaterialName != other.reinforcmentMattressMaterialName) return false;
            if (reinforcmentBlockMaterialName != other.reinforcmentBlockMaterialName) return false;
            if (reinforcmentMaterialWidth != other.reinforcmentMaterialWidth) return false;
            if (composition != other.composition) return false;
            return true;
        }

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            using (PgContext context = new PgContext())
                foreach (var item in context.Mattresses)
                    if (item.perimetrId != null)
                        if (item.perimetrId == perimetrId) list.Add(item);
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => perimetrId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            perimetrId = value;
        }

        public Perimetrs GetCopy() => new Perimetrs
        {
            perimetrId = this.perimetrId,
            reinforcmentMattressMaterialName = this.reinforcmentMattressMaterialName,
            reinforcmentBlockMaterialName = this.reinforcmentBlockMaterialName,
            reinforcmentMaterialWidth = this.reinforcmentMaterialWidth,
            composition = this.composition
        };

        public void Duplicate(Perimetrs obj)
        {
            perimetrId = obj.perimetrId;
            reinforcmentMattressMaterialName = obj.reinforcmentMattressMaterialName;
            reinforcmentBlockMaterialName = obj.reinforcmentBlockMaterialName;
            reinforcmentMaterialWidth = obj.reinforcmentMaterialWidth;
            composition = obj.composition;
        }
    }
}
