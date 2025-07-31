using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lib.DataAccess.Models
{
    [Table("T_Menu")]
    public partial class TMenu
    {
        [Key]
        public int Id { get; set; }
        [Column("classIcon")]
        [StringLength(100)]
        public string ClassIcon { get; set; }
        [Column("classMenu")]
        [StringLength(100)]
        public string ClassMenu { get; set; }
        public int? IdParent { get; set; }
        [Column("isToogle")]
        public bool? IsToogle { get; set; }
        [Column("ignoreCheck")]
        public bool? IgnoreCheck { get; set; }
        [Column("isSelected")]
        public bool? IsSelected { get; set; }
        [Column("routerLink")]
        [StringLength(200)]
        public string RouterLink { get; set; }
        [Column("menuName")]
        [StringLength(100)]
        public string MenuName { get; set; }
        [Column("menuPath")]
        [StringLength(200)]
        public string MenuPath { get; set; }
        [Column("idx")]
        public int? Idx { get; set; }
        [Column("status")]
        public bool? Status { get; set; }
    }
}
