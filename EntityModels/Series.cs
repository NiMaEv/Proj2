using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{ 
    [Table("public.Series")]
    public partial class Series : IEntityExtensions<Series>, ICopying<Series>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Series()
        {
            Mattresses = new HashSet<Mattresses>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int seriesId { get; set; }

        [StringLength(300)]
        public string seriesName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mattresses> Mattresses { get; set; }

        public override string ToString() => seriesName;

        public bool CompareTo(Series other) => seriesName == other.seriesName;

        public int GetId() => seriesId;

        public void SetId(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException();
            seriesId = value;
        }

        public Series GetCopy() => new Series { seriesId = this.seriesId, seriesName = this.seriesName};

        public override int GetHashCode() => seriesId.GetHashCode() * seriesName.GetHashCode() * 1234;

        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
    }
}
