using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace EntityModels
{
    [Table("public.Tables")]
    public partial class Tables
    {
        [Key]
        [StringLength(300)]
        public string tableName { get; set; }

        public override string ToString() => tableName;

    }
}
