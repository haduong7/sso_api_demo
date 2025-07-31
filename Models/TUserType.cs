using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lib.DataAccess.Models
{
    [Table("T_USER_TYPE")]
    public partial class TUserType
    {
        [Key]
        [Column("USER_TYPE_ID")]
        public int UserTypeId { get; set; }
        [Column("USER_TYPE_NAME")]
        [StringLength(50)]
        public string UserTypeName { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
