using System.ComponentModel.DataAnnotations;

namespace ResearchProjectManagementSystem.Payloads
{
    public class ResearchProjectPayload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public int? UserId { get; set; }
    }
}
