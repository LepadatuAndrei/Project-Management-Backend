using System.ComponentModel.DataAnnotations;

namespace ResearchProjectManagementSystem.Payloads
{
    public class StagePayload
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(10000)]
        public string Tasks { get; set; }
        public string? Status { get; set; }
    }
}
