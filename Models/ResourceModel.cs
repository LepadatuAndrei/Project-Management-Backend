using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("Resources")]
    public class ResourceModel
    {
        [Key]
        [Column("Id")]
        public int IdResource { get; set; }
        [Required]
        public string ResourceName { get; set; }
        [Required]
        public string ResourceDescription { get; set; }
        [Required]
        public int TotalAmount { get; set; }
        public List<ResourceAllocationModel> ResourceAllocations { get; set; }
    }
}
