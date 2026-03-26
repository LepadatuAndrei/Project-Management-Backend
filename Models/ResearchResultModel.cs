using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("ResearchResults")]
    public class ResearchResultModel
    {
        [Key]
        [Column("Id")]
        public int IdResult { get; set; }
        [Required]
        public string ResultName { get; set; }
        [Required]
        public DateTime ResultDate { get; set; }
        public int IdProjectStage { get; set; }
        public StageModel ProjectStage { get; set; }
        //ResultFiles...
    }
}
