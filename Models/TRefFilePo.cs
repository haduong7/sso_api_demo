using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_REF_FILE_PO")]
    public class TRefFilePo
    {
        [Key]
        [Column("Id")]
        public int id { get; set; }

        [Column("FileId")]
        public int fileId { get; set; }

        [Column("PoDataId")]
        public int poDataId { get; set; }
    }
}
