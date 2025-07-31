using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSO_Api.Models
{
    [Table("T_PoFlowStage")]
    public class TPoFlowStage
    {
        [Key]
        [Column("PO_FlowStage_ID")]
        public int poFlowStageId { get; set; }

        [Column("PODataId")]
        public int poDataId { get; set; }

        [Column("Step")]
        public int step { get; set; }

        [Column("Performer")]
        [StringLength(100)]
        public string performer { get; set; }

        [Column("DeadlineDays")]
        public int? deadlineDays { get; set; }

        [Column("Description")]
        [StringLength(350)]
        public string? description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CompletionDate { get; set; }

        [Column("Result")]
        public int result { get; set; }

        [Column("PerformComment")]
        [StringLength(350)]
        public string? performComment { get; set; }

        [Column("ApproveLink")]
        [StringLength(350)]
        public string sapapprovelink { get; set; }

        [Column("RejectLink")]
        [StringLength(350)]
        public string saprejectlink { get; set; }
    }
}
