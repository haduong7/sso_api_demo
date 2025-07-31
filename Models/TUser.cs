using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_USER")]
    public class TUser
    {
        [Key]
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("USER_NAME")]
        [StringLength(150)]
        public string UserName { get; set; }

        [Column("PASSWORD")]
        [StringLength(150)]
        public string? Password { get; set; }
        [Column("FULL_NAME")]
        [StringLength(150)]
        public string FullName { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("EMAIL")]
        [StringLength(150)]
        public string Email { get; set; }
        [Column("MOBILE")]
        [StringLength(150)]
        public string? Mobile { get; set; }
        [Column("USER_TYPE")]
        public int? UserType { get; set; }
        [Column("GENDER")]
        public int? Gender { get; set; }
        [Column("BIRTHDAY", TypeName = "datetime")]
        public DateTime? Birthday { get; set; }
        [Column("ID_OR_PASSPORT")]
        [StringLength(30)]
        public string? IdOrPassport { get; set; }
        [Column("ADDRESS")]
        [StringLength(350)]
        public string? Address { get; set; }
        [Column("JOB_DESCRIPTION")]
        [StringLength(350)]
        public string? JobDescription { get; set; }
        [Column("IS_DELETED")]
        public int? IsDeleted { get; set; }
        [Column("AVATAR")]
        [StringLength(300)]
        public string? Avatar { get; set; }
    }
}
