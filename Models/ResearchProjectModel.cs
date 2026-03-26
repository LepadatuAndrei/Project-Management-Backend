using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Projects")]
    public class ResearchProjectModel
    {
        [Key]
        [Column("Id")]
        public int IdProject { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        [Required]
        public string ProjectObjectives { get; set; }
        [Required]
        [Column("CreationDate")]
        public DateTime ProjectCreationDate { get; set; }
        public List<TeamMemberModel> TeamMembers { get; set; }
        public int IdBudget { get; set; }
        public BudgetModel Budget { get; set; }
        public List<ResourceAllocationModel> ResourceAllocations { get; set; }
        public List<StageModel> Stages { get; set; }
        public List<AcquisitionModel> Acquisitions { get; set; }
    }
}
