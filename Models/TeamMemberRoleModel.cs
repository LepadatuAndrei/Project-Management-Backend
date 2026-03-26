using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchProjectManagementSystem.Models
{
    [Table("ResearchTeamMemberRoles")]
    public class TeamMemberRoleModel
    {
        [Key]
        [Column("Id")]
        public int IdRole { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string? RoleDescription { get; set; }
        [Column("IdResearchTeamMember")]
        public int IdTeamMember { get; set; }
        public TeamMemberModel TeamMember { get; set; }
    }
}
