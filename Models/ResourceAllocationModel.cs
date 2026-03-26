using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("ResourceAllocations")]
    public class ResourceAllocationModel
    {
        [Key]
        [Column("Id")]
        public int IdAllocation { get; set; }
        [Required]
        public DateTime AllocationTime { get; set; }
        [Required]
        public DateTime DeallocationTime { get; set; }
        [Required]
        public int Amount { get; set; }
        public int IdResource { get; set; }
        public ResourceModel Resource { get; set; }
        public int IdResearchProject { get; set; }
        public ResearchProjectModel ResearchProject { get; set; }
    }
}
