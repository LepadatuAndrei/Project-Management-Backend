using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Stages")]
    public class StageModel
    {
        [Key]
        [Column("Id")]
        public int IdStage { get; set; }
        [Required]
        [Column("Name")]
        public string StageName { get; set; }
        [Column("Tasks")]
        public string? Tasks { get; set; }
        [Required]
        [Column("Status")]
        public string StageStatus { get; set; }
        public int IdResearchProject { get; set; }
        public ResearchProjectModel ResearchProject { get; set; }
        public int IdBudget { get; set; }
        public BudgetModel Budget { get; set; }
        public List<ReportModel> Reports { get; set; }
        public List<ResearchResultModel> ResearchResults { get; set; }
    }
}
