using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_PO_DATA")]
    public class TPoData
    {
        [Key]
        [Column("PO_DATA_ID")]
        public int PoDataId { get; set; }

        [Column("DocType")]
        [StringLength(50)]
        public string? docType { get; set; }

        [Column("Number")]
        [StringLength(100)]
        public string number { get; set; }

        [Column("Date", TypeName = "datetime")]
        public DateTime date { get; set; }

        [Column("Status")]
        public string status { get; set; }

        [Column("AuthorId")]
        public int authorId { get; set; }

        [Column("Author")]
        [StringLength(100)]
        public string author { get; set; }

        [Column("ResponsibilityId")]
        public int? responsibilityId { get; set; }


        [Column("Responsibility")]
        [StringLength(100)]
        public string? responsibility { get; set; }

        [Column("Company")]
        [StringLength(100)]
        public string company { get; set; }

        [Column("Bu")]
        [StringLength(100)]
        public string? bu { get; set; }

        [Column("Amount", TypeName = "numeric(18, 2)")]
        
        public decimal amount { get; set; }

        [Column("Currency")]
        [StringLength(100)]
        public string currency { get; set; }

        [Column("Description")]
        [StringLength(350)]
        public string description { get; set; }

        [Column("Comment")]
        [StringLength(350)]
        public string? comment { get; set; }

        [Column("HtmlPresentation")]
        [StringLength(350)]
        public string htmlPresentation { get; set; }

        //[Column("Files")]
        //[StringLength(350)]
        //public string? files { get; set; }

        [Column("PreviousDraftID")]
        [StringLength(350)]
        public string? previousDraftID { get; set; }

        [Column("PoStatus")]
        public int? PoStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        public int? ModifyBy { get; set; }
    }
}
