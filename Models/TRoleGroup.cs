using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace SSO_Api.Models
{
    [Table("T_ROLE_GROUP")]
    public partial class TRoleGroup
    {
        [Key]
        [Column("ROLE_GROUP_ID")]
        public int RoleGroupId { get; set; }
        [Column("ROLE_GROUP_NAME")]
        [StringLength(200)]
        public string RoleGroupName { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("CREATED_DATE", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
