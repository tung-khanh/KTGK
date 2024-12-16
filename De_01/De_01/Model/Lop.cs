namespace De_01.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Lop")]
    public partial class Lop
    {
        [Key]
        [StringLength(3)]
        public string MaLop { get; set; }

        [Required]
        [StringLength(30)]
        public string TenLop { get; set; }
    }
}
