using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Lib.DataAccess.Models
{
    [Table("T_REF_ROLE_GROUP_FUNCTION")]
    public partial class TRefRoleGroupFunction
    {
        [Key]
        [Column("ROLE_GROUP_FUNCTION_ID")]
        public int RoleGroupFunctionId { get; set; }
        [Column("ROLE_GROUP_ID")]
        public int? RoleGroupId { get; set; }
        [Column("FUNCTION_ID")]
        public int? FunctionId { get; set; }
        [Column("USER_VIEW")]
        public bool? UserView { get; set; }
        [Column("USER_NEW")]
        public bool? UserNew { get; set; }
        [Column("USER_EDIT")]
        public bool? UserEdit { get; set; }
        [Column("USER_DELETE")]
        public bool? UserDelete { get; set; }
        [Column("USER_APPROVE")]
        public bool? UserApprove { get; set; }
        [Column("USER_REJECT")]
        public bool? UserReject { get; set; }
    }
}
