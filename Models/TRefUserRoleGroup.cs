using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lib.DataAccess.Models
{
    [Table("T_REF_USER_ROLE_GROUP")]
    public partial class TRefUserRoleGroup
    {
        [Key]
        [Column("USER_ROLE_GROUP_ID")]
        public int UserRoleGroupId { get; set; }
        [Column("USER_ID")]
        public int? UserId { get; set; }
        [Column("ROLE_GROUP_ID")]
        public int? RoleGroupId { get; set; }
    }
}
