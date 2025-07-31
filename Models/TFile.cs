using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_FILE")]
    public class TFile
    {
        [Key]
        [Column("FILE_ID")]
        public int FileId { get; set; }

        [Column("SERVICE")]
        [StringLength(350)]
        public string? service { get; set; }

        [Column("NAME")]
        [StringLength(350)]
        public string? name { get; set; }

        [Column("LINK")]
        [StringLength(350)]
        public string link { get; set; }
    }
}
