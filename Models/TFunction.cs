using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lib.DataAccess.Models
{
    [Table("T_FUNCTION")]
    public partial class TFunction
    {
        [Key]
        [Column("FUNCTION_ID")]
        public int FunctionId { get; set; }
        [Column("FUNCTION_NAME")]
        [StringLength(100)]
        public string FunctionName { get; set; }
        [Column("DISPLAY_NAME")]
        [StringLength(150)]
        public string DisplayName { get; set; }
        [Column("PARENT_ID")]
        public int? ParentId { get; set; }
        [Column("PAGE_URL")]
        [StringLength(500)]
        public string PageUrl { get; set; }
        [Column("MENU_ID")]
        public int? MenuId { get; set; }
        [Column("TYPE")]
        [StringLength(10)]
        public string Type { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("CREATED_DATE", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }
        [Column("ICON")]
        [StringLength(100)]
        public string Icon { get; set; }
    }
}
