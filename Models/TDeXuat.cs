using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_DeXuat")]
    public class TDeXuat
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("TenDeXuat")]
        [StringLength(200)]
        public string TenDeXuat { get; set; }

        [Column("LoaiDeXuat")]
        public int? LoaiDeXuat { get; set; }

        [Column("Status")]
        public int? Status { get; set; }

        [Column("GhiChu")]
        [StringLength(500)]
        public string GhiChu { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        public int? ModifyBy { get; set; }
    }
}
