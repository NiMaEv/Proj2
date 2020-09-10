using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Sizes")]
    public partial class Sizes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sizeId { get; set; }

        public int lenght { get; set; }

        public int width { get; set; }

        public override string ToString() => $"{lenght}*{width}";
    }
}
