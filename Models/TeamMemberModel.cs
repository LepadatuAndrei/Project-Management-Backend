using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("ResearchTeamMembers")]
    public class TeamMemberModel
    {
        [Key]
        [Column("Id")]
        public int IdTeamMember { get; set; }
        [Required]
        [Column("JoinDate")]
        public DateTime TeamJoinDate { get; set; }
        public int IdResearcher { get; set; }
        public UserModel Researcher { get; set; }
        public List<TeamMemberTaskModel> TeamMemberTasks { get; set; }
        public int IdResearchProject { get; set; }
        public ResearchProjectModel ResearchProject { get; set; }
        public TeamMemberRoleModel? TeamMemberRole { get; set; }
    }
}
