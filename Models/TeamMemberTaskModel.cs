using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("ResearchMemberTasks")]
    public class TeamMemberTaskModel
    {
        [Key]
        [Column("Id")]
        public int IdTask { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string TaskDescription { get; set; }
        [Required]
        [Column("CreationDate")]
        public DateTime TaskCreationDate { get; set; }
        [Column("Deadline")]
        public DateTime? TaskDeadline { get; set; }
        [Required]
        [Column("Status")]
        public string TaskStatus { get; set; }
        [Column("IdResearchMember")]
        public int IdTeamMember { get; set; }
        public TeamMemberModel TeamMember { get; set; }
    }
}
