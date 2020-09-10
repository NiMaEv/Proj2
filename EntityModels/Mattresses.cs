using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Mattresses")]
    public partial class Mattresses
    {
        [Key]
        [StringLength(300)]
        public string mattressName { get; set; }

        public int? seriesId { get; set; }

        public int? topperId { get; set; }

        public int compositionId { get; set; }

        public int cutId { get; set; }

        public int? burletId { get; set; }

        public int? perimetrId { get; set; }

        public virtual Burlets Burlets { get; set; }

        public virtual Cuts Cuts { get; set; }

        public virtual MtrsCompositions MtrsCompositions { get; set; }

        public virtual Perimetrs Perimetrs { get; set; }

        public virtual Series Series { get; set; }

        public virtual Toppers Toppers { get; set; }
    }
}
