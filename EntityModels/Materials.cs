using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Materials")]
    public partial class Materials
    {
        [Key]
        [StringLength(300)]
        public string materialName { get; set; }

        public int? materialHeight { get; set; }

        [StringLength(300)]
        public string sectorName { get; set; }
    }
}
