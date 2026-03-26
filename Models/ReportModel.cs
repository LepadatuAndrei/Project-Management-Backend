using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Reports")]
    public class ReportModel
    {
        [Key]
        [Column("Id")]
        public int IdReport { get; set; }
        [Required]
        public string ReportName { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public int IdProjectStage { get; set; }
        public StageModel ProjectStage { get; set; }
        //ReportFiles...
    }
}
