using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Burlets")]
    public partial class Burlets : IEntityExtensions<Burlets>, IEntityAdvanceExnensions, ICopying<Burlets>, IDuplication<Burlets>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Burlets()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int burletId { get; set; }

        [Required]
        [StringLength(500)]
        public string composition { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        public bool CompareTo(Burlets other) => composition == other.composition & description == other.description;

        public bool GetDependentEntities(out List<Mattresses> list)
        {
            list = new List<Mattresses>();
            using (PgContext context = new PgContext())
                foreach (Mattresses item in context.Mattresses)
                    if (item.burletId != null)
                        if (item.burletId == burletId) list.Add(item);
            return list.Count > 1 /*!= 0*/;
        }

        public int GetId() => burletId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            burletId = value;
        }

        public Burlets GetCopy() => new Burlets { burletId = this.burletId, composition = this.composition, description = this.description };

        public void Duplicate(Burlets obj)
        {
            burletId = obj.burletId;
            composition = obj.composition;
            description = obj.description;
        }
    }
}
